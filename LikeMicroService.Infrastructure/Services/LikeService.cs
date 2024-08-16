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

        // Test Code
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

        //public async Task CreateLikeAsync(int textId)
        //{
        //    var userTask = GetUserRequestDataAsync();
        //    var likeTask = GetLikeAsync(textId);

        //    await Task.WhenAll(userTask, likeTask);

        //    if (userTask.Result == null) { return; }

        //    if (likeTask.Result != null) { return; }

        //    var createTask = _likeRepository.CreateAsync(textId, userTask.Result.Id);
        //    var editTask = _httpHelper.EditTextLikeAsync(Core.Enums.LikeAction.Plus, textId);

        //    await Task.WhenAll(createTask, editTask);
        //}

        //public async Task DeleteLikeAsync(int textId)
        //{
        //    var userTask = GetUserRequestDataAsync();
        //    var likeTask = GetLikeAsync(textId);

        //    await Task.WhenAll(userTask, likeTask);

        //    if (userTask.Result == null) { return; }

        //    if (likeTask.Result == null) { return; }

        //    var editTask = _httpHelper.EditTextLikeAsync(Core.Enums.LikeAction.Minus, textId);
        //    var deleteTask = _likeRepository.DeleteAsync(textId, userTask.Result.Id);

        //    await Task.WhenAll(editTask, deleteTask);
        //}

        public async Task<Like?> GetLikeAsync(int textId)
        {
            var user = await GetUserRequestDataAsync();

            return await _likeRepository.GetAsync(textId, user.Id);
        }

        private async Task<UserDTO?> GetUserRequestDataAsync()
        {
            var userName = _httpContextAccessor.HttpContext?.User.FindFirst(JwtRegisteredClaimNames.Name)?.Value;

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
