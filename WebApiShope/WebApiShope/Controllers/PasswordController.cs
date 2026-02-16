using DTO;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Services;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiShope.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PasswordController : ControllerBase
    {
        private readonly IPasswordsService _passwordService; 


        public PasswordController(IPasswordsService passwordService)
        {
            this._passwordService = passwordService;
        }
        // GET: api/<PasswordController>


        // POST api/<PasswordController>
        [HttpPost]
        public ActionResult<int> CheckPasswordStrength([FromBody] PasswordDTO password)
        {
           Resulte<int> respone=_passwordService.CheckPasswordStrength(password);
            if (!respone.IsSuccess ) 
            {
                return BadRequest(respone.ErrorMessage);
            }
            return Ok(respone.Data);
        }


        // PUT api/<PasswordController>/5

    }
}
