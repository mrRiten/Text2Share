using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using TextMicroService.Application.Helpers;
using TextMicroService.Application.Repositories;
using TextMicroService.Application.Services;
using TextMicroService.Core.Models;
using UserMicroService.Core.Models;

namespace TextMicroService.Infrastructure.Services
{
    public class TextService(ITextRepository textRepository, IHttpContextAccessor httpContextAccessor, 
        IHttpHelper httpHelper) : ITextService
    {
        private readonly ITextRepository _textRepository = textRepository;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IHttpHelper _httpHelper = httpHelper;

        public async Task CreateTextAsync(TextUpload model)
        {
            var randomBytes = GenerateRandomBytes(16);
            var text = new Text
            {
                Data = model.Data,
                UserId = model.UserId,
                IsPublic = model.IsPublic,
                DateOfCreate = DateTime.UtcNow,
                DateOfChange = DateTime.UtcNow,
                PrivetToken = ConvertToHex(randomBytes)
            };

            await _textRepository.CreateAsync(text);
        }

        public async Task DeleteTextAsync(int id)
        {
            var text = await _textRepository.GetAsync(id);

            if (text == null) return;

            await _textRepository.DeleteAsync(text);
        }

        public async Task<ICollection<Text>> GetAllTextAsync(bool isAdmin)
        {
            var texts = isAdmin
                ? await _textRepository.AdminGetAllAsync()
                : await _textRepository.GetAllAsync();

            return RemovePrivateTokens(texts);
        }

        public async Task<ICollection<Text>> GetAllTextByUserAsync(int userId, bool isAdmin)
        {
            var texts = isAdmin
                ? await _textRepository.GetAllByUserAsync(userId)
                : await _textRepository.GetAllPublicByUserAsync(userId);

            return RemovePrivateTokens(texts);
        }

        public async Task<ICollection<Text>?> GetAllUserTextAsync()
        {
            var user = await GetUserRequestDataAsync();
            if (user == null) return null;

            var texts = await _textRepository.GetAllByUserAsync(user.Id);
            return RemovePrivateTokens(texts);
        }

        public async Task<Text?> GetTextAsync(int id, bool isAdmin)
        {
            return isAdmin
                ? await _textRepository.GetAsync(id)
                : await _textRepository.GetPublicAsync(id);
        }

        public async Task<Text?> GetTextAsync(string privetToken)
        {
            return await _textRepository.GetAsync(privetToken);
        }

        public bool IsAllowedPath(string path)
        {
            return path.Equals("/Data", StringComparison.OrdinalIgnoreCase) ||
                   path.Equals("/DateOfChange", StringComparison.OrdinalIgnoreCase) ||
                   path.Equals("/IsPublic", StringComparison.OrdinalIgnoreCase) ||
                   path.Equals("/LikeCount", StringComparison.OrdinalIgnoreCase);
        }

        public async Task UpdateTextAsync(Text text)
        {
            await _textRepository.UpdateAsync(text);
        }

        private async Task<UserDTO?> GetUserRequestDataAsync()
        {
            var userName = _httpContextAccessor.HttpContext?.User.FindFirst(JwtRegisteredClaimNames.Name)?.Value;
            if (userName == null) return null;

            var response = await _httpHelper.GetUserAsync(userName);
            return JsonConvert.DeserializeObject<UserDTO>(await response.Content.ReadAsStringAsync());
        }

        private static byte[] GenerateRandomBytes(int length)
        {
            var randomBytes = new byte[length];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }
            return randomBytes;
        }

        private static string ConvertToHex(byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", "");
        }

        private static ICollection<Text> RemovePrivateTokens(ICollection<Text> texts)
        {
            foreach (var text in texts)
            {
                text.DeletePrivetToken();
            }
            return texts;
        }
    }
}
