using LikeMicroService.Application.Repositories;
using LikeMicroService.Core;
using LikeMicroService.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace LikeMicroService.Infrastructure.Repositories
{
    public class LikeRepository(LikeMicroServiceContext context) : ILikeRepository
    {
        private readonly LikeMicroServiceContext _context = context;

        public async Task CreateAsync(int textId, int userId)
        {
            var like = new Like { TextId = textId, UserId = userId };

            await _context.Likes.AddAsync(like);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int textId, int userId)
        {
            var like = await GetAsync(textId, userId);

            if (like == null) { return; }

            _context.Likes.Remove(like);
            await _context.SaveChangesAsync();
        }

        public async Task<Like?> GetAsync(int textId, int userId)
        {
            return await _context.Likes.FirstOrDefaultAsync(l => l.TextId == textId || l.UserId == userId);
        }
    }
}
