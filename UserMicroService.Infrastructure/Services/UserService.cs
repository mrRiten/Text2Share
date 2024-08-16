using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using UserMicroService.Application.Helpers;
using UserMicroService.Application.Repositories;
using UserMicroService.Application.Services;
using UserMicroService.Core.Models;

namespace UserMicroService.Infrastructure.Services
{
    public class UserService(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor,
        IHttpHelper httpHelper) : IUserService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task CreateUserAsync(User user)
        {
            await _userRepository.CreateAsync(user);
        }

        public async Task<User?> GetFullUserAsync()
        {
            return await GetUserRequestDataAsync();
        }

        public async Task<User?> GetFullUserAsync(string userName)
        {
            return await _userRepository.GetAsync(userName);
        }

        public async Task<UserDTO?> GetUserAsync(string userName)
        {
            var user = await _userRepository.GetAsync(userName);

            if (user == null) { return null; }

            var userDTO = new UserDTO(user);

            return userDTO;
        }

        public async Task<UserDTO?> GetUserAsync(int userId)
        {
            var user = await _userRepository.GetAsync(userId);

            if (user == null) { return null; }

            var userDTO = new UserDTO(user);

            return userDTO;
        }

        public async Task<UserDTO?> GetUserAsync()
        {
            var user = await GetUserRequestDataAsync();

            if (user == null) { return null; }

            var userDTO = new UserDTO(user);

            return userDTO;
        }

        public bool IsAllowedPath(string path)
        {
            return path.Equals("/UserName", StringComparison.OrdinalIgnoreCase);
        }

        public async Task<bool> SetNewPassword(string oldPassword, string newPassword, User user)
        {
            if (!BCrypt.Net.BCrypt.Verify(oldPassword, user.Password)) { return false; }

            user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
            await _userRepository.UpdateAsync(user);

            return true;
        }

        public async Task UpdateUserAsync(User user)
        {
            await _userRepository.UpdateAsync(user);
        }

        private async Task<User?> GetUserRequestDataAsync()
        {
            var userName = _httpContextAccessor.HttpContext?.User.FindFirst(JwtRegisteredClaimNames.Name)?.Value;

            if (userName == null) { return null; }

            var user = await _userRepository.GetAsync(userName);

            return user;
        }
    }
}
