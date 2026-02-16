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

        private readonly IGeminiServise _geminiServise;

        public GeminiController(IGeminiServise geminiServise)
        {
            this._geminiServise = geminiServise;    
        }

        // GET: api/<GeminiController>
        [HttpPost("userProduct")]
        public   async  Task<ActionResult<GeminiPrompt>> CreateUserPromptForProduct(long categoryId, string userRequest)
        {
            Resulte<GeminiPrompt> resulte= await _geminiServise.AddGeminiForUserProductServise(categoryId, userRequest);
            if (!resulte.IsSuccess)
            {
                if(resulte.ErrorMessage.Equals("Server error"))
                {
                    return Problem("faild to load gemini try again");
                }
                 return BadRequest(resulte.ErrorMessage);
            }
            return Ok(resulte.Data);
        }


        // GET: api/<GeminiController>
        [HttpPost("category")]
        public async Task<ActionResult<GeminiPrompt>> CreateUserPromptCategory(string userRequest, long categoryId)
        {
            Resulte<GeminiPrompt> resulte = await _geminiServise.AddGeminiForUserFillCategoryServise( userRequest, categoryId);
            if (!resulte.IsSuccess)
            {
                if (resulte.ErrorMessage.Equals("Server error"))
                {
                    return Problem("faild to load gemini try again");
                }
                return BadRequest(resulte.ErrorMessage);
            }
            return Ok(resulte.Data);
        }


        // GET: api/<GeminiController>
        [HttpPost("basicSite")]
        public async Task<ActionResult<GeminiPrompt>> CreateUserPromptBasicSite(string userRequest)
        {
            Resulte<GeminiPrompt> resulte = await _geminiServise.AddGeminiForUserFillBasicSiteServise(userRequest);
            if (!resulte.IsSuccess)
            {
                if (resulte.ErrorMessage.Equals("Server error"))
                {
                    return Problem("faild to load gemini try again");
                }
                return BadRequest(resulte.ErrorMessage);
            }
            return Ok(resulte.Data);
        }
        // GET api/<GeminiController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GeminiPrompt>> Get(long id)
        {
            GeminiPrompt? gemini = await _geminiServise.GetByIdPromptServise(id);

            if(gemini == null)
            {
                return NoContent();
            }
            return Ok(gemini);
        }

     
        // PUT api/<GeminiController>/5
        [HttpPut("{id}/userProduct")]
        public async Task<ActionResult> UpdatePromptForProduct(long promptId, string userRequest)
        {

            Resulte<GeminiPrompt> respone= await _geminiServise.AddGeminiForUserProductServise(promptId, userRequest);
            if(!respone.IsSuccess)
            {
                if(respone.ErrorMessage.Equals("Server error"))
                {
                  return Problem("faild to load gemini try again");
                }
                return BadRequest(respone.ErrorMessage);
            }
            return Ok();

        }



        // PUT api/<GeminiController>/5
        [HttpPut("{id}/basicSite")]
        public async Task<ActionResult> UpdatePromptBasicSite(long promptId, string userRequest)
        {

            Resulte<GeminiPrompt> respone = await _geminiServise.UpdateGeminiForUserBasicSiteServise(promptId, userRequest);
            if (!respone.IsSuccess)
            {
                if (respone.ErrorMessage.Equals("Server error"))
                {
                    return Problem("faild to load gemini try again");
                }
                return BadRequest(respone.ErrorMessage);
            }
            return Ok();

        }

        // PUT api/<GeminiController>/5
        [HttpPut("{id}/category")]
        public async Task<ActionResult> UpdatePromptCategory(long promptId, string userRequest)
        {

            Resulte<GeminiPrompt> respone = await _geminiServise.UpdateGeminiForUserCategoryServise(promptId, userRequest);
            if (!respone.IsSuccess)
            {
                if (respone.ErrorMessage.Equals("Server error"))
                {
                    return Problem("faild to load gemini try again");
                }
                return BadRequest(respone.ErrorMessage);
            }
            return Ok();
        }
        // DELETE api/<GeminiController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult>  Delete(long id)
        {
            Resulte<GeminiPrompt> respone =await _geminiServise.DeletePromptServise(id);
            if (!respone.IsSuccess)
            {
                BadRequest(respone.ErrorMessage);
            }
            return Ok();    
        }
    }
}
