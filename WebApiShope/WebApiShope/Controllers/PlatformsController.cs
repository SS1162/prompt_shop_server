using DTO;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiShope.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformsServise _platformsServise;
        public PlatformsController(IPlatformsServise platformsServise) {
            this._platformsServise = platformsServise;
        }
        // GET: api/<PlatformsController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlatformsDTO>>> Get()
        {
           IEnumerable < PlatformsDTO > platformList = await _platformsServise.GetPlatformsServise();
            if (platformList.Any())
            {
                return NoContent();
            }
            else
                return Ok(platformList);
        }

     

        // POST api/<PlatformsController>
        [HttpPost]
        public async Task<ActionResult<PlatformsDTO>> AddPlatform([FromBody] AddPlatformDTO platform)
        {
          
           PlatformsDTO PlatformForReturn= await _platformsServise.AddPlatformServise(platform);

            return CreatedAtAction(nameof(Get), new { id = PlatformForReturn.PlatformID }, PlatformForReturn);
        }

        // PUT api/<PlatformsController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdatePlatform(long id, [FromBody] PlatformsDTO platform)
        {
            Resulte<PlatformsDTO>  respone= await _platformsServise.UpdatePlatformServise(id,platform);
            if (!respone.IsSuccess)
            {
                return BadRequest(respone.ErrorMessage);
            }
            return Ok();
        }

        // DELETE api/<PlatformsController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(long id)
        {
            Resulte<PlatformsDTO> respone = await _platformsServise.DeletePlatformServise(id);
            if (!respone.IsSuccess)
            {
                return BadRequest(respone.ErrorMessage);
            }
            return Ok();
        }
    }
}
