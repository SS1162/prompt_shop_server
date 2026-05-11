using DTO;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Services;
using WebApiShope.Attributes;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiShope.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsServise _productsServise;

        public ProductsController(IProductsServise productsServise)
        {
            this._productsServise = productsServise; 
        }
        // GET: api/<ProductsController>
        [HttpGet]
        async public Task<ActionResult<Resulte<ResponePage<ProductDTO>>>> Get(long categoryID, int numOfPages, int PageSize, string? search, int? minPrice, int? MaxPrice, bool? orderByPrice, bool? desc)
        {
            Resulte<ResponePage<ProductDTO>> respone = await _productsServise.GetProductsServise( categoryID,  numOfPages,  PageSize,  search,  minPrice,   MaxPrice,  orderByPrice,   desc);
           if(!respone.IsSuccess)
            {
                return BadRequest(respone.ErrorMessage);
            }
           if(!respone.Data.Data.Any())
            {
                return NoContent();
            }
            return Ok(respone.Data);
        }

    
        // POST api/<ProductsController>
        [AdminOnly]
        [HttpPost]
        async public Task<ActionResult<ProductDTO>> AddProduct([FromBody] AddProductDTO product)
        {

            Resulte<ProductDTO> reaspone = await _productsServise.AddProductServise(product);
            if (!reaspone.IsSuccess)
            { 
                return BadRequest(reaspone.ErrorMessage);
            }
            return CreatedAtAction(nameof(Get), new { id = reaspone.Data.ProductsID }, reaspone.Data);
        
        }

        // PUT api/<ProductsController>/5
        [AdminOnly]
        [HttpPut("{id}")]
        async public Task<ActionResult> UpdateProduct(long id, [FromBody] UpdateProductDTO productToUpdate )
        {
            Resulte<ProductDTO> reaspone = await _productsServise.UpdateProductServise(id, productToUpdate);
            if (!reaspone.IsSuccess)
            {
                return BadRequest(reaspone.ErrorMessage);
            }
            return Ok();
        }

        // DELETE api/<ProductsController>/5
        [AdminOnly]
        [HttpDelete("{id}")]
        async public Task<ActionResult> DeleteProduct(long id)
        {

            Resulte<ProductDTO> reaspone = await _productsServise.DeleteIDProductServise(id);
            if (!reaspone.IsSuccess)
            {
                return BadRequest(reaspone.ErrorMessage);
            }
            return Ok();
        }


        [HttpGet("all")]
        async public Task<ActionResult<IEnumerable<ProductDTO>>> GetAll()
        {
            IEnumerable<ProductDTO> response = await _productsServise.GetAllProductServise();
            if(!response.Any())
            {
                return NoContent();
            }
            return Ok(response);
        }
    }
    
}
