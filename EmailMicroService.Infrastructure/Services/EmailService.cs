using EmailMicroService.Application.Repositories;
using EmailMicroService.Application.Services;
using EmailMicroService.Core.Models;

namespace EmailMicroService.Infrastructure.Services
{
    public class EmailService(IEmailRepository emailRepository) : IEmailService
    {
        private readonly IEmailRepository _emailRepository = emailRepository;

        public async Task CreateEmailAsync(Email email)
        {
            await _emailRepository.CreateAsync(email);
        }
    }
}
