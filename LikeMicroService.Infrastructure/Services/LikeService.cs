using LikeMicroService.Application.Helpers;
using LikeMicroService.Application.Repositories;
using LikeMicroService.Application.Services;
using LikeMicroService.Core.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace LikeMicroService.Infrastructure.Services
{
    public class LikeService(ILikeRepository likeRepository, IHttpContextAccessor httpContextAccessor,
        IHttpHelper httpHelper) : ILikeService
    {
        private readonly ILikeRepository _likeRepository = likeRepository;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IHttpHelper _httpHelper = httpHelper;

        public async Task CreateLikeAsync(int textId)
        {
            var userTask = GetUserRequestDataAsync();
            var likeTask = GetLikeAsync(textId);

            await userTask;
            await likeTask;

            await Task.WhenAll();

            if (userTask.Result == null) { return; }

            if (likeTask.Result != null) { return; }

            await _likeRepository.CreateAsync(textId, userTask.Result.IdUser);
            await _httpHelper.EditTextLikeAsync(Core.Enums.LikeAction.Plus, textId);
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
