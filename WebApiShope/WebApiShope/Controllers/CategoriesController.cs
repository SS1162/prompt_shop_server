using Microsoft.AspNetCore.Mvc;
using Services;
using DTO;
using Microsoft.AspNetCore.Http.HttpResults;
using StackExchange.Redis;
using System.Text;
using System.Text.Json;
using WebApiShope.Attributes;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiShope.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoriesServise _categoriesServise;
        private readonly IConnectionMultiplexer _redis;
        private readonly CategoryCacheOptions _categoryCacheOptions;
        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public CategoriesController(ICategoriesServise categoriesServise, IConnectionMultiplexer redis, CategoryCacheOptions categoryCacheOptions)
        {
            this._categoriesServise = categoriesServise;
            _redis = redis;
            _categoryCacheOptions = categoryCacheOptions;
        }
        // GET: api/<CategoryController>
        [HttpGet]
        async public Task<ActionResult<ResponePage<CategoryDTO>>> GetCategoriesByMainCategoryID(int numberOfPages, long mainCategoryID, int pageSize, string? search)
        {
            long version = 1;
            try
            {
                version = await GetMainCategoryVersionAsync(_redis.GetDatabase(), mainCategoryID);
            }
            catch
            {
                // Fail open.
            }

            string cacheKey = BuildCategoriesListCacheKey(mainCategoryID, numberOfPages, pageSize, search, version);

            try
            {
                IDatabase cache = _redis.GetDatabase();
                RedisValue cachedPage = await cache.StringGetAsync(cacheKey);
                if (!cachedPage.IsNullOrEmpty)
                {
                    ResponePage<CategoryDTO>? pageFromCache = JsonSerializer.Deserialize<ResponePage<CategoryDTO>>(cachedPage!, _jsonOptions);
                    if (pageFromCache != null)
                    {
                        if (!pageFromCache.Data.Any())
                        {
                            return NoContent();
                        }

                        return Ok(pageFromCache);
                    }
                }
            }
            catch
            {
                // Fail open: continue to DB path when Redis is unavailable.
            }

            Resulte<ResponePage<CategoryDTO>> respone = await _categoriesServise.GetCategoriesServise(numberOfPages, mainCategoryID, pageSize, search);
            if (!respone.IsSuccess)
            {
                return BadRequest(respone.ErrorMessage);
            }
            if (!respone.Data.Data.Any())
            {
                return NoContent();
            }

            try
            {
                IDatabase cache = _redis.GetDatabase();
                int ttlSeconds = _categoryCacheOptions.TtlSeconds <= 0 ? 120 : _categoryCacheOptions.TtlSeconds;
                await cache.StringSetAsync(cacheKey, JsonSerializer.Serialize(respone.Data, _jsonOptions), TimeSpan.FromSeconds(ttlSeconds));
            }
            catch
            {
                // Fail open.
            }

            return Ok(respone.Data);
        }

        // GET api/<CategoryController>/5
        [HttpGet("{id}")]
        async public Task<ActionResult<CategoryDTO>> GetCategoryByCategoryID(long id)
        {
            CategoryDTO? category = await _categoriesServise.GetByIDCategoriesServise(id);
            if (category == null)
            {
                return NoContent();
            }

            return Ok(category);
        }

        // POST api/<CategoryController>
        [AdminOnly]
        [HttpPost]
        async public Task<ActionResult<CategoryDTO>> AddCategory([FromForm] AddCategoryDTO category)
        {
            Resulte<CategoryDTO> categoryConstructedObject = await _categoriesServise.AddCategoriesServise(category);
            if (!categoryConstructedObject.IsSuccess)
            {
                return BadRequest(categoryConstructedObject.ErrorMessage);
            }

            await InvalidateMainCategoryCachesAsync(categoryConstructedObject.Data.MainCategoryID);

            return CreatedAtAction(nameof(GetCategoryByCategoryID), new { id = categoryConstructedObject.Data.CategoryID }, categoryConstructedObject.Data);
        }

        // PUT api/<CategoryController>/5
        [AdminOnly]
        [HttpPut("{id}")]
        async public Task<ActionResult> UpdateCategory(long id, [FromForm] CategoryToUpdateDTO category)
        {
            CategoryDTO? categoryBeforeUpdate = await _categoriesServise.GetByIDCategoriesServise(id);

            Resulte<CategoryDTO> respone = await _categoriesServise.UpdateCategoriesServise(id, category);
            if (!respone.IsSuccess)
            {
                return BadRequest(respone.ErrorMessage);
            }

            if (categoryBeforeUpdate != null)
            {
                await InvalidateMainCategoryCachesAsync(categoryBeforeUpdate.MainCategoryID, category.MainCategoryID);
            }
            else
            {
                await InvalidateMainCategoryCachesAsync(category.MainCategoryID);
            }

            return Ok();
        }

        // DELETE api/<CategoryController>/5
        [AdminOnly]
        [HttpDelete("{id}")]
        async public Task<ActionResult> DeleteCategoty(long id)
        {
            CategoryDTO? categoryBeforeDelete = await _categoriesServise.GetByIDCategoriesServise(id);

            Resulte<CategoryDTO> respone = await _categoriesServise.DeleteIDCategoriesServise(id);
            if (!respone.IsSuccess)
            {
                return BadRequest(respone.ErrorMessage);
            }

            if (categoryBeforeDelete != null)
            {
                await InvalidateMainCategoryCachesAsync(categoryBeforeDelete.MainCategoryID);
            }

            return Ok();
        }

        private static string BuildMainCategoryVersionKey(long mainCategoryId) => $"categories:list:main:{mainCategoryId}:version";

        private static string BuildCategoriesListCacheKey(long mainCategoryID, int numberOfPages, int pageSize, string? search, long version)
        {
            string searchPart = Convert.ToBase64String(Encoding.UTF8.GetBytes(search ?? string.Empty));
            return $"categories:list:main:{mainCategoryID}:v:{version}:page:{numberOfPages}:size:{pageSize}:search:{searchPart}";
        }

        private static async Task<long> GetMainCategoryVersionAsync(IDatabase cache, long mainCategoryId)
        {
            RedisValue versionValue = await cache.StringGetAsync(BuildMainCategoryVersionKey(mainCategoryId));
            if (!versionValue.IsNullOrEmpty && long.TryParse(versionValue.ToString(), out long parsedVersion))
            {
                return parsedVersion;
            }

            return 1;
        }

        private async Task InvalidateMainCategoryCachesAsync(params long[] mainCategoryIds)
        {
            try
            {
                IDatabase cache = _redis.GetDatabase();
                foreach (long mainCategoryId in mainCategoryIds.Distinct())
                {
                    await cache.StringIncrementAsync(BuildMainCategoryVersionKey(mainCategoryId));
                }
            }
            catch
            {
                // Fail open.
            }
        }

    }
}

