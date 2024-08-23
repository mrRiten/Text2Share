using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using UserMicroService.Application.Services;
using UserMicroService.Core.Attributes;
using UserMicroService.Core.Models;

namespace UserMicroService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserService userService, IImageService imageService, 
        IOptions<XSource> source, IWebHostEnvironment webEnvironment) : ControllerBase
    {
        private readonly IUserService _userService = userService;
        private readonly IImageService _imageService = imageService;
        private readonly XSource _source = source.Value;
        private readonly IWebHostEnvironment _webHostEnvironment = webEnvironment;

        // GET: api/<UserController>/1
        [HttpGet("{userId:int}")]
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
            if (HttpContext.Request.Headers["X-Source"] == _source.Token)
            {
                var user = await _userService.GetFullUserAsync(userName);

                if (user == null) { return NotFound(); }

                return Ok(user);
            }
            else
            {
                var user = await _userService.GetUserAsync(userName);

                if (user == null) { return NotFound(); }

                return Ok(user);
            }
        }

        // POST: api/<UserController>/
        [HttpPost]
        [ServiceFilter(typeof(ValidateSourceFilter))]
        public async Task<IActionResult> Post(User user)
        {
            if (ModelState.IsValid)
            {
                await _userService.CreateUserAsync(user);
                var newUser = await _userService.GetUserAsync(user.UserName);

                return Ok(newUser);
            }

            return BadRequest();
        }

        [HttpPost("Image")]
        [Authorize]
        public async Task<IActionResult> PostImage([FromForm] UploadUserImage userImage)
        {
            try
            {
                var user = await _userService.GetFullUserAsync();
                if (user == null)
                    return NotFound("User not found");

                var imageUrl = await _imageService.UploadUserImageAsync(userImage.Image);
                user.UserImagePath = imageUrl;

                await _userService.UpdateUserAsync(user);
                return Ok("User's image updated");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("ChangePassword")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromForm] string newPassword, 
            [FromForm] string oldPassword)
        {
            var user = await _userService.GetFullUserAsync();
            if (user == null)
                return NotFound("User not found");

            var isPasswordChanged = await _userService.SetNewPasswordAsync(oldPassword, newPassword, user);
            if (!isPasswordChanged)
                return BadRequest("Old password is not valid");

            return Ok("Password has been changed");
        }

        // PATCH: api/<UserController>/update
        [HttpPatch]
        [Authorize]
        public async Task<IActionResult> Patch([FromBody] JsonPatchDocument<User> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest("Patch document is null");

            if (HttpContext.Request.Headers["X-Source"] != _source.Token)
            {
                if (HasInvalidPatchOperations(patchDoc))
                    return BadRequest("Modification of one or more fields is not allowed");
            }

            var user = await _userService.GetFullUserAsync();
            if (user == null)
                return NotFound();

            patchDoc.ApplyTo(user, ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _userService.UpdateUserAsync(user);
            return Ok();
        }

        private bool HasInvalidPatchOperations(JsonPatchDocument<User> patchDoc)
        {
            return patchDoc.Operations.Any(op => !_userService.IsAllowedPath(op.path));
        }
    }
}
