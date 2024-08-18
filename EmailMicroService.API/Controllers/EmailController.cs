using EmailMicroService.Application.Services;
using EmailMicroService.Core.Attributes;
using EmailMicroService.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmailMicroService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController(IEmailService emailService) : ControllerBase
    {
        private readonly IEmailService _emailService = emailService
            ?? throw new ArgumentNullException(nameof(emailService));

        // POST api/<EmailController>
        [HttpPost]
        [ServiceFilter(typeof(ValidateSourceFilter))]
        public async Task<IActionResult> Post(Email email)
        {
            await _emailService.CreateEmailAsync(email);

            return Ok();
        }
    }
}
