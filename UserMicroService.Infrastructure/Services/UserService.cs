using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
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
        private readonly IHttpHelper _httpHelper = httpHelper;

        public async Task CreateUserAsync(User user)
        {
            await _userRepository.CreateAsync(user);
        }

        public async Task<User?> GetUserAsync(string userName)
        {
            return await _userRepository.GetAsync(userName);
        }

        public async Task<User?> GetUserAsync(int userId)
        {
            return await _userRepository.GetAsync(userId);
        }

        public async Task<User?> GetUserAsync()
        {
            return await GetUserRequestDataAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            await _userRepository.UpdateAsync(user);
        }

        private async Task<User?> GetUserRequestDataAsync()
        {
            var userName = _httpContextAccessor.HttpContext?.User.FindFirst(JwtRegisteredClaimNames.Name)?.Value;

            var response = await _httpHelper.GetUserAsync(userName);

            if (response.IsSuccessStatusCode)
            {
                var userJson = await response.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<User>(userJson);

                return user;
            }

            return null;
        }
    }
}
