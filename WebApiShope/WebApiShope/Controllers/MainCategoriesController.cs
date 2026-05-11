using DTO;
using Microsoft.AspNetCore.Mvc;
using Services;
using System.Reflection.Metadata.Ecma335;
using WebApiShope.Attributes;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiShope.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MainCategoriesController : ControllerBase
    {
        private readonly IMainCategoriesServise _mainCategoriesServise;
        public MainCategoriesController(IMainCategoriesServise mainCategoriesServise)
        {
            this._mainCategoriesServise = mainCategoriesServise;
        }

        // GET: api/<MainCategoriesController>
        [HttpGet]
        async public Task<ActionResult<IEnumerable<MainCategoriesDTO>>> Get()
        {
            IEnumerable<MainCategoriesDTO> MainCategories = await _mainCategoriesServise.GetMainCategoriesServises();
            if (!MainCategories.Any())
            {
                return NoContent();
            }
            return Ok(MainCategories);
        }


        // POST api/<MainCategoriesController>
        [AdminOnly]
        [HttpPost("admin")]
        async public Task<ActionResult<MainCategoriesDTO>> AddMainCategory([FromBody] ManegerMainCategoryDTO manegerMainCategory)
        {
            MainCategoriesDTO mainCategoryConstructedObject = await _mainCategoriesServise.AddMainCategoriesServises(manegerMainCategory);
            return CreatedAtAction(nameof(Get), new { id = mainCategoryConstructedObject.MainCategoryID }, mainCategoryConstructedObject); 
        }

        // PUT api/<MainCategoriesController>/5
        [AdminOnly]
        [HttpPut("admin/{id}")]
        async public Task<ActionResult> UpdateMainCategory(long id, [FromBody] MainCategoriesDTO mainCategory)
        {
            Resulte<MainCategoriesDTO> respone= await _mainCategoriesServise.UpdateMainCategoriesServises(id, mainCategory);
            if(!respone.IsSuccess)
            {
                return BadRequest(respone.ErrorMessage);    
            }
            return Ok();
        }

        // DELETE api/<MainCategoriesController>/5
        [AdminOnly]
        [HttpDelete("admin/{id}")]
        async public Task<ActionResult> Delete(long id)
        {
            Resulte<MainCategoriesDTO> respone = await _mainCategoriesServise.DeleteMainCategoriesServises(id);
            if (!respone.IsSuccess)
            {
                return BadRequest(respone.ErrorMessage);
            }
            return BadRequest();
        }
    }
}
