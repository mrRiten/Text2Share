using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using UserMicroService.Application.Services;
using UserMicroService.Core.Models;

namespace UserMicroService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserService userService, IOptions<XSource> source) : ControllerBase
    {
        private readonly IUserService _userService = userService;
        private readonly XSource _source = source.Value;

        // GET: api/<UserController>/1
        [HttpGet("{userId}")]
        public async Task<IActionResult> Get(int userId)
        {
            var user = await _userService.GetUserAsync(userId);

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
            if (HttpContext.Request.Headers["X-Source"] != _source.Token) { return BadRequest(); }

            if (ModelState.IsValid)
            {
                await _userService.CreateUserAsync(user);
                var newUser = await _userService.GetUserAsync(user.UserName);

                return Ok(newUser);
            }

            return BadRequest();
        }

        // PATCH: api/<UserController>/update
        [HttpPatch]
        [Authorize]
        public async Task<IActionResult> Patch([FromBody] JsonPatchDocument<User> patchDoc)
        {
            var user = await _userService.GetUserAsync();

            if (user == null) { return NotFound(); }

            patchDoc.ApplyTo(user, (patchError) =>
            {
                ModelState.AddModelError(patchError.AffectedObject.ToString(), patchError.ErrorMessage);
            });

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _userService.UpdateUserAsync(user);

            return Ok();
        }

    }
}
