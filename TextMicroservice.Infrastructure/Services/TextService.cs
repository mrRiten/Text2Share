using System.Security.Cryptography;
using TextMicroService.Application.Repositories;
using TextMicroService.Application.Services;
using TextMicroService.Core.Models;

namespace TextMicroService.Infrastructure.Services
{
    public class TextService(ITextRepository textRepository) : ITextService
    {
        private readonly ITextRepository _textRepository = textRepository;

        public async Task CreateTextAsync(TextUpload model)
        {
            byte[] randomBytes = new byte[32 / 2];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }

            var text = new Text
            {
                Data = model.Data,
                UserId = model.UserId,
                IsPublic = model.IsPublic,
                DateOfCreate = DateTime.Now,
                DateOfChange = DateTime.Now,
                PrivetToken = BitConverter.ToString(randomBytes).Replace("-", "")
        };

            await _textRepository.CreateAsync(text);
        }

        public async Task DeleteTextAsync(int id)
        {
            var text = await _textRepository.GetAsync(id);

            if (text == null) { return; }

            await _textRepository.DeleteAsync(text);
        }

        public async Task<ICollection<Text>> GetAllTextAsync(bool isAdmin)
        {
            if (isAdmin)
            {
                return await _textRepository.AdminGetAllAsync();
            }
            else
            {
                return await _textRepository.GetAllAsync();
            }
        }

        public async Task<ICollection<Text>> GetAllTextByUserAsync(int userId, bool isAdmin)
        {
            if (isAdmin)
            {
                return await _textRepository.GetAllByUserAsync(userId);
            }
            else
            {
                return await _textRepository.GetAllPublicByUserAsync(userId);
            }
        }

        public async Task<Text?> GetTextAsync(int id)
        {
            return await _textRepository.GetAsync(id);
        }

        public async Task UpdateTextAsync(int id, TextUpload model)
        {
            var text = await _textRepository.GetAsync(id);
            
            if (text == null) { return; }

            text.Data = model.Data;
            text.IsPublic = model.IsPublic;
            text.UserId = model.UserId;
            text.DateOfChange = DateTime.Now;

            await _textRepository.UpdateAsync(text);
        }
    }
}
