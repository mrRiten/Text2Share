using TextMicroService.Core.Models;

namespace TextMicroService.Application.Repositories
{
    public interface ITextRepository
    {
        public Task<Text?> GetAsync(int id);
        public Task<Text?> GetPublicAsync(int id);
        public Task<Text?> GetAsync(string token);
        public Task<ICollection<Text>> GetAllAsync();
        public Task<ICollection<Text>> AdminGetAllAsync();
        public Task<ICollection<Text>> GetAllByUserAsync(int userId);
        public Task<ICollection<Text>> GetAllPublicByUserAsync(int userId);

        public Task CreateAsync(Text text);
        public Task UpdateAsync(Text text);
        public Task DeleteAsync(Text text);
    }
}
