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
            ICollection<Text> texts;
            if (isAdmin)
            {
                texts = await _textRepository.AdminGetAllAsync();
            }
            else
            {
                texts = await _textRepository.GetAllAsync();
            }

            foreach (var text in texts)
            {
                text.DeletePrivetToken();
            }

            return texts;
        }

        public async Task<ICollection<Text>> GetAllTextByUserAsync(int userId, bool isAdmin)
        {
            ICollection<Text> texts;
            if (isAdmin)
            {
                texts = await _textRepository.GetAllByUserAsync(userId);
            }
            else
            {
                texts = await _textRepository.GetAllPublicByUserAsync(userId);
            }

            foreach (var text in texts)
            {
                text.DeletePrivetToken();
            }

            return texts;
        }

        public async Task<ICollection<Text>> GetAllUserTextAsync(int userId)
        {
            var texts = await _textRepository.GetAllByUserAsync(userId);

            foreach (var text in texts)
            {
                text.DeletePrivetToken();
            }

            return texts;
        }

        public async Task<Text?> GetTextAsync(int id, bool isAdmin)
        {
            Text? text;

            if (isAdmin)
            {
                text = await _textRepository.GetAsync(id);
            }
            else
            {
                text = await _textRepository.GetPublicAsync(id);
            }

            if (text != null) { return text; }

            return text;
        }

        public async Task<Text?> GetTextAsync(string privetToken)
        {
            return await _textRepository.GetAsync(privetToken);
        }

        public async Task UpdateTextAsync(Text text)
        {
            await _textRepository.UpdateAsync(text);
        }
    }
}
