using EmailMicroService.Application.Services;
using EmailMicroService.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmailMicroService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController(IEmailService emailService) : ControllerBase
    {
        private readonly IEmailService _emailService = emailService;

        // POST api/<EmailController>
        [HttpPost]
        public async Task<IActionResult> Post(Email email)
        {
            await _emailService.CreateEmailAsync(email);

            return Ok();
        }
    }
}
