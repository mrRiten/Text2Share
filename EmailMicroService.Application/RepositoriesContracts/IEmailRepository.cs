using EmailMicroService.Core.Models;

namespace EmailMicroService.Application.Repositories
{
    public interface IEmailRepository
    {
        public Task CreateAsync(Email email);
        public Task<ICollection<Email>> GetAllAsync();
        public Task DeleteAsync(Email[] emails);
    }
}
