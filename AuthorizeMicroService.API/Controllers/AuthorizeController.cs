using AuthorizeMicroService.Application.Helpers;
using AuthorizeMicroService.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using AuthorizeMicroService.Core.Models;

namespace AuthorizeMicroService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizeController(IAuthorizeService authorizeService, HttpClient httpClient,
        IJwtHelper jwtHelper) : ControllerBase
    {
        private readonly IAuthorizeService _authorizeService = authorizeService;
        private readonly HttpClient _httpClient = httpClient;
        private readonly IJwtHelper _jwtHelper = jwtHelper;

        // POST: api/<AuthorizeController>/Login
        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLogin userLogin)
        {
            if (ModelState.IsValid)
            {
                // Query to User Service for get user
                var response = await _httpClient.GetAsync($"https://localhost:7240/api/User?userName={userLogin.Login}");

                if (response == null)
                {
                    return NotFound();
                }

                var userJson = await response.Content.ReadAsStringAsync();

                var result = _authorizeService.VerifyUser(userLogin, userJson);

                if (result)
                {
                    var jwtToken = _jwtHelper.GenerateJwtToken(userLogin.Login);
                    return Ok(jwtToken);
                }

                return NotFound();
            }

            return BadRequest(ModelState);
        }

        // POST: api/<AuthorizeController>/Register
        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserUpload userUpload)
        {
            if (ModelState.IsValid) 
            {
                var user = _authorizeService.BuildUser(userUpload);

                var content = new StringContent(user, Encoding.UTF8, "application/json");

                // Query to User Service for create user
                var response = await _httpClient.PostAsync("https://localhost:7240/api/User", content);
                
                if (response.IsSuccessStatusCode)
                {
                    return Ok("User registered successfully.");
                }

                return BadRequest("Registration failed");  
            }

            return BadRequest(ModelState);
        }

    }
}
