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

        public async Task ModifyLikeAsync(int textId, bool isLike)
        {
            var userTask = GetUserRequestDataAsync();
            var likeTask = GetLikeAsync(textId);

            var user = await userTask;
            var like = await likeTask;

            if (user == null || (isLike && like != null) || (!isLike && like == null))
            {
                return;
            }

            var likeAction = isLike ? Core.Enums.LikeAction.Plus : Core.Enums.LikeAction.Minus;
            
            var repositoryTask = isLike ? _likeRepository.CreateAsync(textId, user.Id)
                                        : _likeRepository.DeleteAsync(textId, user.Id);
            var editTask = _httpHelper.EditTextLikeAsync(likeAction, textId);

            await Task.WhenAll(repositoryTask, editTask);
        }

        public Task CreateLikeAsync(int textId)
        {
            return ModifyLikeAsync(textId, true);
        }

        public Task DeleteLikeAsync(int textId)
        {
            return ModifyLikeAsync(textId, false);
        }

        public async Task<Like?> GetLikeAsync(int textId)
        {
            var user = await GetUserRequestDataAsync();
            ArgumentNullException.ThrowIfNull(user);

            return await _likeRepository.GetAsync(textId, user.Id);
        }

        private async Task<UserDTO?> GetUserRequestDataAsync()
        {
            var userName = _httpContextAccessor.HttpContext?.User.FindFirst(JwtRegisteredClaimNames.Name)?.Value;
            ArgumentNullException.ThrowIfNull(userName);

            var response = await _httpHelper.GetUserAsync(userName);

            if (response.IsSuccessStatusCode)
            {
                var userJson = await response.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<UserDTO>(userJson);

                return user;
            }

            return null;
        }
    }
}
