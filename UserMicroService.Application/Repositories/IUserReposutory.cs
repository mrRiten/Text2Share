using UserMicroService.Core.Models;

namespace UserMicroService.Application.Repositories
{
    public interface IUserRepository
    {
        public Task<User?> GetAsync(int id);
        public Task<User?> GetAsync(string userName);

        public Task CreateAsync(User user);
        public Task UpdateAsync(User user);
    }
}
