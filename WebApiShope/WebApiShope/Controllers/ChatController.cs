using Microsoft.AspNetCore.Mvc;
using Services;
using DTO;

namespace WebApiShope.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatBotServise _chatBotServise;

        public ChatController(IChatBotServise chatBotServise)
        {
          this._chatBotServise = chatBotServise;
        }

        [HttpPost]
        public async Task<ActionResult<string>> SendMessage([FromBody] ChatRequestDto request)
        {
            
          Resulte<string> result = await _chatBotServise.SendMessage(request);

            if (!result.IsSuccess)
            {
                if (result.ErrorMessage == "Server error")
                {
                    return Problem("Failed to process chat request. Please try again later.");
                }
                return BadRequest(result.ErrorMessage);
            }

            return Ok(result.Data);
        }
    }
}
