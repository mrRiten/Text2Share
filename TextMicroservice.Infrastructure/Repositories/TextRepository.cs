using Microsoft.EntityFrameworkCore;
using TextMicroService.Application.Repositories;
using TextMicroService.Core;
using TextMicroService.Core.Models;

namespace TextMicroService.Infrastructure.Repositories
{
    public class TextRepository(TextMicroServiceContext context) : ITextRepository
    {
        private readonly TextMicroServiceContext _context = context;

        public async Task<ICollection<Text>> AdminGetAllAsync()
        {
            return await _context.Texts.ToListAsync();
        }

        public async Task CreateAsync(Text text)
        {
            await _context.Texts.AddAsync(text);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Text text)
        {
            _context.Texts.Remove(text);
            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<Text>> GetAllAsync()
        {
            return await _context.Texts.Where(t => t.IsPublic == true).ToListAsync();
        }

        public async Task<ICollection<Text>> GetAllByUserAsync(int userId)
        {
            return await _context.Texts
                .Where(t => t.UserId == userId)
                .ToListAsync();
        }

        public async Task<ICollection<Text>> GetAllPublicByUserAsync(int userId)
        {
            return await _context.Texts
                .Where(t => t.UserId == userId && t.IsPublic == true)
                .ToListAsync();
        }

        public async Task<Text?> GetAsync(int id)
        {
            return await _context.Texts.FindAsync(id);
        }

        public async Task<Text?> GetAsync(string token)
        {
            return await _context.Texts
                .FirstOrDefaultAsync(t => t.PrivetToken == token);
        }

        public async Task UpdateAsync(Text text)
        {
            _context.Update(text);
            await _context.SaveChangesAsync();
        }

    }
}
