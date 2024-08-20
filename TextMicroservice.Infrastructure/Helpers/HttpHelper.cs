using TextMicroService.Application.Helpers;

namespace TextMicroService.Infrastructure.Helpers
{
    public class HttpHelper(HttpClient httpClient) : IHttpHelper
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<HttpResponseMessage> GetUserAsync(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentException("User name cannot be null or whitespace.", nameof(userName));
            }

            return await _httpClient.GetAsync($"https://localhost:7000/api/User?userName={userName}");
        }
    }
}
