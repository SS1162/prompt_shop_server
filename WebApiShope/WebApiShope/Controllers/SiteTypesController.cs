using DTO;
using Microsoft.AspNetCore.Mvc;
using Services;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiShope.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SiteTypesController : ControllerBase
    {
        private readonly ISiteTypesService _siteTypesService;
        public SiteTypesController(ISiteTypesService siteTypesService)
        {
            this._siteTypesService = siteTypesService;
        }
        // GET: api/<SiteTypeController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SiteTypeDTO>?>> GetAll()
        {
            var siteTypes = await _siteTypesService.GetAllSiteTypesServise();
            if (!siteTypes.Any() )
            {
                return NotFound();
            }
            return Ok(siteTypes);
        }


        // GET api/<SiteTypeController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SiteTypeDTO>> GetById(long id)
        {
            SiteTypeDTO? siteType = await _siteTypesService.GetSiteTypesByIdServise(id);
            if(siteType == null )
                return NotFound();
            return Ok(siteType);
        }

        // POST api/<SiteTypeController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        [HttpPut("{id}/manager")]
        public async Task<ActionResult> UpdateByMng(long id, SiteTypeDTO dto)
        {

             Resulte<SiteTypeDTO> reaspone=await _siteTypesService.UpdateSiteTypesByMngServise(id, dto);
            if(!reaspone.IsSuccess)
            {
                return BadRequest(reaspone.ErrorMessage);
            }
            return Ok();    
        }


    }
}
