using Microsoft.AspNetCore.JsonPatch;
using TextMicroService.Core.Models;

namespace TextMicroService.Application.Services
{
    public interface ITextService
    {
        public Task<Text?> GetTextAsync(int id, bool isAdmin);
        public Task<Text?> GetTextAsync(string privetToken);
        public Task<ICollection<Text>> GetAllUserTextAsync(int userId);
        public Task<ICollection<Text>> GetAllTextAsync(bool isAdmin);
        public Task<ICollection<Text>> GetAllTextByUserAsync(int userId, bool isAdmin);

        public Task<string?> GetTextTokenAsync(int textId);

        public Task CreateTextAsync(TextUpload model);
        public Task UpdateTextAsync(Text text);
        public Task DeleteTextAsync(int id);
    }
}
