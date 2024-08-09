using UserMicroService.Core.Models;

namespace UserMicroService.Application.Services
{
    public interface IUserService
    {
        public Task<User?> GetUserAsync(int userId);
        public Task<User?> GetUserAsync(string userName);
        public Task<User?> GetUserAsync();

        public Task CreateUserAsync(User user);
        public Task UpdateUserAsync(User user);
    }
}
