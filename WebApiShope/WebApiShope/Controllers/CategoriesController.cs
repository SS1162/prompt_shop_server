using Microsoft.AspNetCore.Mvc;
using Services;
using DTO;
using Microsoft.AspNetCore.Http.HttpResults;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiShope.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoriesServise _categoriesServise;
        public CategoriesController(ICategoriesServise categoriesServise)
        {
            this._categoriesServise = categoriesServise;
        }
        // GET: api/<CategoryController>
        [HttpGet]
        async public Task<ActionResult<ResponePage<CategoryDTO>>> GetCategoriesByMainCategoryID(int numberOfPages, long mainCategoryID, int pageSize, string? search)
        {
            Resulte<ResponePage<CategoryDTO>> respone = await _categoriesServise.GetCategoriesServise(numberOfPages, mainCategoryID, pageSize, search);
            if (!respone.IsSuccess)
            {
                return BadRequest(respone.ErrorMessage);
            }
            if (!respone.Data.Data.Any())
            {
                return NoContent();
            }

            return Ok(respone.Data);
        }

        // GET api/<CategoryController>/5
        [HttpGet("{id}")]
        async public Task<ActionResult<CategoryDTO>> GetCategoryByCategoryID(long id)
        {
            CategoryDTO category = await _categoriesServise.GetByIDCategoriesServise(id);
            if (category == null)
            {
                return NoContent();
            }
            return Ok(category);
        }

        // POST api/<CategoryController>
        [HttpPost]
        async public Task<ActionResult<CategoryDTO>> AddCategory([FromForm] AddCategoryDTO category)
        {
            Resulte<CategoryDTO> categoryConstructedObject = await _categoriesServise.AddCategoriesServise(category);
            if (!categoryConstructedObject.IsSuccess)
            {
                return BadRequest(categoryConstructedObject.ErrorMessage);
            }

            return CreatedAtAction(nameof(GetCategoryByCategoryID), new { id = categoryConstructedObject.Data.CategoryID }, categoryConstructedObject.Data);
        }

        // PUT api/<CategoryController>/5
        [HttpPut("{id}")]
        async public Task<ActionResult> UpdateCategory(long id, [FromForm] CategoryToUpdateDTO category)
        {
            Resulte<CategoryDTO> respone = await _categoriesServise.UpdateCategoriesServise(id, category);
            if (!respone.IsSuccess)
            {
                return BadRequest(respone.ErrorMessage);
            }
            return Ok();
        }

        // DELETE api/<CategoryController>/5
        [HttpDelete("{id}")]
        async public Task<ActionResult> DeleteCategoty(long id)
        {
            Resulte<CategoryDTO> respone = await _categoriesServise.DeleteIDCategoriesServise(id);
            if (!respone.IsSuccess)
            {
                return BadRequest(respone.ErrorMessage);
            }
            return Ok();
        }




    }
}

