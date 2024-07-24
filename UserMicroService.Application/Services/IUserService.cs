using UserMicroService.Core.Models;

namespace UserMicroService.Application.Services
{
    public interface IUserService
    {
        public User? GetUser(int userId);
        public Task<User?> GetUserAsync(string userName);

        public Task CreateUserAsync(User user);
    }
}
