using LikeMicroService.Core.Models;

namespace LikeMicroService.Application.Repositories
{
    public interface ILikeRepository
    {
        public Task CreateAsync(int textId, int userId);
        public Task<Like?> GetAsync(int textId, int userId);
        public Task DeleteAsync(int textId, int userId);
    }
}
