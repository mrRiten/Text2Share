using UserMicroService.Core.Models;

namespace UserMicroService.Application.Services
{
    public interface IUserService
    {
        public Task<UserDTO?> GetUserAsync(int userId);
        public Task<UserDTO?> GetUserAsync(string userName);
        public Task<UserDTO?> GetUserAsync();

        public Task<User?> GetFullUserAsync();
        public Task<User?> GetFullUserAsync(string userName);

        public bool IsAllowedPath(string path);

        public Task CreateUserAsync(User user);
        public Task UpdateUserAsync(User user);
    }
}
