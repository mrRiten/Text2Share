using LikeMicroService.Core.Models;

namespace LikeMicroService.Application.Services
{
    public interface ILikeService
    {
        public Task<Like?> GetLikeAsync(int textId);
        public Task CreateLikeAsync(int textId);
        public Task DeleteLikeAsync(int textId);
    }
}
