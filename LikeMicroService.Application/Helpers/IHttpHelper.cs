using LikeMicroService.Core.Enums;

namespace LikeMicroService.Application.Helpers
{
    public interface IHttpHelper
    {
        public Task<HttpResponseMessage> GetUserAsync(string userName);
        public Task<HttpResponseMessage?> EditTextLikeAsync(LikeAction likeAction, int textId);
    }
}
