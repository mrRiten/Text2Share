using LikeMicroService.Application.Helpers;
using LikeMicroService.Application.Repositories;
using LikeMicroService.Application.Services;
using LikeMicroService.Core.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace LikeMicroService.Infrastructure.Services
{
    public class LikeService(ILikeRepository likeRepository, IHttpContextAccessor httpContextAccessor,
        IHttpHelper httpHelper) : ILikeService
    {
        private readonly ILikeRepository _likeRepository = likeRepository;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IHttpHelper _httpHelper = httpHelper;

        // ToDo: Create getways validate system for identification source of requast (Source token)
        // ToDo: Create method for update like count with request to UserMicroService

        public async Task CreateLikeAsync(int textId)
        {
            var user = await GetUserRequestDataAsync();

            if (user == null) { return; }

            var like = await GetLikeAsync(textId);

            if (like != null) { return; }

            await _likeRepository.CreateAsync(textId, user.IdUser);
        }

        public async Task DeleteLikeAsync(int textId)
        {
            var user = await GetUserRequestDataAsync();

            if (user == null) { return; }

            await _likeRepository.DeleteAsync(textId, user.IdUser);
        }

        public async Task<Like?> GetLikeAsync(int textId)
        {
            var user = await GetUserRequestDataAsync();

            return await _likeRepository.GetAsync(textId, user.IdUser);
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
