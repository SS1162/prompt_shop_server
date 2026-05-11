using DTO;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Services;
using System.Threading.Tasks;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiShope.Controllers
{   [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BasicSiteController : ControllerBase
    {
        private readonly IBasicSitesServise _basicSitesServise;
        public BasicSiteController(IBasicSitesServise basicSitesServise)
        {
            this._basicSitesServise = basicSitesServise;

        }

      
        // GET api/<BasicSiteController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BasicSiteDTO>> GetBasicSiteByID(long id)
        {
            BasicSiteDTO basicSite = await _basicSitesServise.GetByIDbasicSiteServise(id);
            if (basicSite == null)
            {
                return NoContent();
            }
            return Ok(basicSite);
        }

        // POST api/<BasicSiteController>
        [HttpPost]
        async public Task<ActionResult<BasicSiteDTO>> AddBasicSite([FromBody] AddBasicSiteDTO basicSite)
        {
           Resulte<BasicSiteDTO> basicSiteConstructedObject = await _basicSitesServise.AddBasicSiteServise(basicSite);
            if (!basicSiteConstructedObject.IsSuccess)
            {
                return BadRequest(basicSiteConstructedObject.ErrorMessage);
            }
            else
                return CreatedAtAction(nameof(GetBasicSiteByID), new { id = basicSiteConstructedObject.Data.BasicSiteID }, basicSiteConstructedObject.Data);
        
        }

        // PUT api/<BasicSiteController>/5
        [HttpPut("{id}")]
        async public Task<ActionResult> UpdateBasicSite(long id, [FromBody] UpdateBasicSiteDTO basicSite)
        {
            Resulte<BasicSiteDTO?> resulteFromController = await _basicSitesServise.UpdateBasicSiteServise(id, basicSite);
            if (!resulteFromController.IsSuccess)
            {
                return BadRequest(resulteFromController.ErrorMessage);
            }
            else return Ok();

        }

       
    }
}
