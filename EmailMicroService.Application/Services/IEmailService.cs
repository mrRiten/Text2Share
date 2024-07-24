using EmailMicroService.Core.Models;

namespace EmailMicroService.Application.Services
{
    public interface IEmailService
    {
        public Task CreateEmailAsync(Email email);
    }
}
