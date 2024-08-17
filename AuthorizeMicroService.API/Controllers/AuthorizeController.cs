﻿using AuthorizeMicroService.Application.Helpers;
using AuthorizeMicroService.Application.Services;
using AuthorizeMicroService.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace AuthorizeMicroService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizeController(IAuthorizeService authorizeService, HttpClient httpClient,
        IJwtHelper jwtHelper, IHttpHelper httpHelper) : ControllerBase
    {
        private readonly IAuthorizeService _authorizeService = authorizeService;
        private readonly HttpClient _httpClient = httpClient;
        private readonly IJwtHelper _jwtHelper = jwtHelper;
        private readonly IHttpHelper _httpHelper = httpHelper;

        // POST: api/<AuthorizeController>/Confirm/{userId}?token={token}
        [HttpGet("Confirm/{userId}")]
        public async Task<IActionResult> Confirm(int userId, string token)
        {
            await _authorizeService.Confirm(userId, token);

            return Ok();
        }

        // POST: api/<AuthorizeController>/Login
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromForm] UserLogin userLogin)
        {
            if (ModelState.IsValid)
            {
                var user = await _authorizeService.GetUserAsync(userLogin.Login);

                if (user == null) { return NotFound("User is not found"); }

                var result = _authorizeService.VerifyUser(userLogin, user);

                if (result)
                {
                    var jwtToken = _jwtHelper.GenerateJwtToken(userLogin.Login);
                    return Ok(jwtToken);
                }

                return BadRequest("Password is not valid");
            }

            return BadRequest(ModelState);
        }

        // POST: api/<AuthorizeController>/Register
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromForm] UserUpload userUpload)
        {
            if (ModelState.IsValid)
            {
                var user = _authorizeService.BuildUser(userUpload);

                await _authorizeService.CreateUserAsync(user);
                user = await _authorizeService.GetUserAsync(user.UserName); // Get user with ID

                if (user == null) { return BadRequest("Registration failed"); }

                var response = await _httpHelper.CreateEmailAsync(user);

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
