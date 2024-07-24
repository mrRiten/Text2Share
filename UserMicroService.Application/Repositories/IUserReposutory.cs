using UserMicroService.Core.Models;

namespace UserMicroService.Application.Repositories
{
    public interface IUserRepository
    {
        public User? Get(int id);
        public Task<User?> GetAsync(string userName);

        public Task CreateAsync(User user);
    }
}
