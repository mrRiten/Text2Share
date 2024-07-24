using UserMicroService.Application.Repositories;
using UserMicroService.Application.Services;
using UserMicroService.Core.Models;

namespace UserMicroService.Infrastructure.Services
{
    public class UserService(IUserRepository userRepository) : IUserService
    {
        private readonly IUserRepository _userRepository = userRepository;

        public async Task CreateUserAsync(User user)
        {
            await _userRepository.CreateAsync(user);
        }

        public User? GetUser(int userId)
        {
            return _userRepository.Get(userId);
        }

        public async Task<User?> GetUserAsync(string userName)
        {
            return await _userRepository.GetAsync(userName);
        }
    }
}
