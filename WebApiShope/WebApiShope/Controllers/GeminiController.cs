using Microsoft.AspNetCore.Mvc;
using Services;
using DTO;
using Entities;
using Google.GenAI;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiShope.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeminiController : ControllerBase
    {

        IGeminiServise _geminiServise;

        public GeminiController(IGeminiServise geminiServise)
        {
            this._geminiServise = geminiServise;    
        }

        // GET: api/<GeminiController>
        [HttpGet("getUserProduct")]
        public   async  Task<ActionResult<GeminiPrompt>> CreateUserPromptForProduct(long productId ,string userRequest)
        {
            Resulte<GeminiPrompt> resulte= await _geminiServise.AddGeminiForUserProductServise(productId, userRequest);
            if (!resulte.IsSuccess)
                return BadRequest(resulte.ErrorMessage);
            if(resulte.Data==null)
            {
                return Problem ("faild to load gemini try again");
            }
            return Ok(resulte.Data);
        }

        // GET api/<GeminiController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GeminiPrompt>> Get(int id)
        {
            GeminiPrompt? gemini = await _geminiServise.GetByIdPromptServise(id);

            if(gemini == null)
            {
                return NoContent();
            }
            return Ok(gemini);
        }

        //// POST api/<GeminiController>)
        //[HttpPost("addNewProduct")]
        //public void Post([FromBody] string value)
        //{

        //}

        // PUT api/<GeminiController>/5
        [HttpPut("{id}")]
        public async Task UpdatePrompt(long promptId, string userRequest)
        {

            await _geminiServise.AddGeminiForUserProductServise(promptId, userRequest);

        }
        //// DELETE api/<GeminiController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
