using DTO;
using Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NLog.Web;
using Services;
using System.Text.Json;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiShope.Controllers
{
  

    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

      

        private readonly ILogger<UsersController> _logger;

        public UsersController(IUsersService usersService, ILogger<UsersController> logger)
        {
            this._usersService = usersService;
           
            this._logger = logger;  
        }

        [HttpGet]
        public async Task<bool> CheckIfTheUserInsist(string userName)
        {
            return await _usersService.CheckIfUsersInsistalradyServise(userName);
        }






        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUserById(long id)
        {

            UserDTO user = await _usersService.GetByIDUsersService(id);
            if (user == null)
            {
                return NoContent();
            }
            else
                return user;
        }


        // POST api/<UsersController>

        //POST request

        [HttpPost("loginFunction")]
        
        public async Task<ActionResult<UserDTO>> PostLogin([FromBody] LoginUserDTO logInUser)
        {
            UserDTO user = await _usersService.LoginUsersService(logInUser);
            if (user == null)
            {
                return Unauthorized();

            }
            else
            {
                _logger.LogInformation($"login with:user name:{user.UserName} user firs name:{user.FirstName} user las name:{user.LastName},user phone:{user.Phone},user ID:{user.UserID}");
                return CreatedAtAction(nameof(GetUserById), new { id = user.UserID}, user);
            }

        }

        [HttpPost("signInWithGoogle")]
        public async Task<ActionResult<UserDTO>> SignInWithGoogle([FromBody] RegisterUserDTO userFromUser)
        {
           UserDTO reaspne = await _usersService.SignInWithGoogleServise(userFromUser);
         
            return Ok(reaspne);
        }
        [HttpPost]
        public async Task<ActionResult<UserDTO>> AddNewUser([FromBody] RegisterUserDTO userFromUser)
        {
            Resulte<UserDTO> reaspne = await _usersService.AddNewUsersService(userFromUser);
            if(!reaspne.IsSuccess)
            {
                return   BadRequest(reaspne.ErrorMessage);
            }
                return CreatedAtAction(nameof(GetUserById), new { id = reaspne.Data.UserID }, reaspne.Data);
        }
        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        async public Task<ActionResult> Put(long id, [FromBody] UpdateUserDTO user)
        {

            Resulte < UserDTO >  reaspone= await _usersService.UpdateUsersService(id, user);
            if (!reaspone.IsSuccess)
            {
                return BadRequest(reaspone.ErrorMessage);
            }
            return Ok();
        }


    }
}
