using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using UserMicroService.Application.Helpers;
using UserMicroService.Application.Repositories;
using UserMicroService.Application.Services;
using UserMicroService.Core.Models;

namespace UserMicroService.Infrastructure.Services
{
    public class UserService(IUserRepository userRepository, 
        IHttpContextAccessor httpContextAccessor) : IUserService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task CreateUserAsync(User user)
        {
            await _userRepository.CreateAsync(user);
        }

        public async Task<UserDTO?> GetUserAsync(string userName)
        {
            var user = await _userRepository.GetAsync(userName);
            return user == null ? null : new UserDTO(user);
        }

        public async Task<UserDTO?> GetUserAsync(int userId)
        {
            var user = await _userRepository.GetAsync(userId);
            return user == null ? null : new UserDTO(user);
        }

        public async Task<UserDTO?> GetUserAsync()
        {
            var user = await GetUserRequestDataAsync();
            return user == null ? null : new UserDTO(user);
        }

        public async Task<User?> GetFullUserAsync(string userName)
        {
            return await _userRepository.GetAsync(userName);
        }

        public async Task<User?> GetFullUserAsync()
        {
            return await GetUserRequestDataAsync();
        }

        public bool IsAllowedPath(string path)
        {
            return path.Equals("/UserName", StringComparison.OrdinalIgnoreCase);
        }

        public async Task<bool> SetNewPasswordAsync(string oldPassword, string newPassword, User user)
        {
            if (!VerifyOldPassword(oldPassword, user.Password))
            {
                return false;
            }

            user.Password = HashNewPassword(newPassword);
            await _userRepository.UpdateAsync(user);

            return true;
        }

        public async Task UpdateUserAsync(User user)
        {
            await _userRepository.UpdateAsync(user);
        }

        private async Task<User?> GetUserRequestDataAsync()
        {
            var userName = GetUserNameFromClaims();
            return userName == null ? null : await _userRepository.GetAsync(userName);
        }

        private string? GetUserNameFromClaims()
        {
            return _httpContextAccessor.HttpContext?.User.FindFirst(JwtRegisteredClaimNames.Name)?.Value;
        }

        private bool VerifyOldPassword(string oldPassword, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(oldPassword, hashedPassword);
        }

        private string HashNewPassword(string newPassword)
        {
            return BCrypt.Net.BCrypt.HashPassword(newPassword);
        }
    }
}
