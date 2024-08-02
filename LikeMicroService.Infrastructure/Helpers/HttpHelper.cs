using LikeMicroService.Application.Helpers;

namespace LikeMicroService.Infrastructure.Helpers
{
    public class HttpHelper(HttpClient httpClient) : IHttpHelper
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<HttpResponseMessage> GetUserAsync(string userName)
        {
            return await _httpClient.GetAsync($"https://localhost:7000/api/User?userName={userName}");
        }
    }
}
