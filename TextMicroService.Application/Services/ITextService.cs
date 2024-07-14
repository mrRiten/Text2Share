using TextMicroService.Core.Models;

namespace TextMicroService.Application.Services
{
    public interface ITextService
    {
        public Task<Text?> GetTextAsync(int id);
        public Task<ICollection<Text>> GetAllTextAsync(bool isAdmin);
        public Task<ICollection<Text>> GetAllTextByUserAsync(int userId, bool isAdmin);

        public Task CreateTextAsync(TextUpload model);
        public Task UpdateTextAsync(int id, TextUpload model);
        public Task DeleteTextAsync(int id);
    }
}
