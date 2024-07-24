using EmailMicroService.Core.Models;

namespace EmailMicroService.Application.Repositories
{
    public interface IEmailRepository
    {
        public Task CreateAsync(Email email);
    }
}
