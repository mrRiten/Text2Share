using Microsoft.AspNetCore.Mvc;
using UserMicroService.Application.Services;
using UserMicroService.Core.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UserMicroService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;

        // GET: api/<UserController>/1
        [HttpGet("{userId}")]
        public IActionResult Get(int userId)
        {
            var user = _userService.GetUser(userId);

            if (user == null) { return NotFound(); }

            return Ok(user);
        }

        // GET: api/<UserController>?userName=Alice
        [HttpGet]
        public async Task<IActionResult> Get(string userName)
        {
            var user = await _userService.GetUserAsync(userName);

            if (user == null) { return NotFound(); }

            return Ok(user);
        }

        // POST: api/<UserController>/
        [HttpPost]
        public async Task<IActionResult> Post(User user)
        {
            if (ModelState.IsValid)
            {
                await _userService.CreateUserAsync(user);
                
                return Ok("RegisterSuccess");
            }

            return BadRequest();
        }

    }
}
