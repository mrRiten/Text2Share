using AuthorizeMicroService.Application.Helpers;
using AuthorizeMicroService.Core.Models;
using Newtonsoft.Json;
using System.Text;

namespace AuthorizeMicroService.Infrastructure.Helpers
{
    public class HttpHelper(HttpClient httpClient) : IHttpHelper
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<HttpResponseMessage> CreateEmailAsync(User user)
        {
            var email = new Email
            {
                UserEmail = user.UserEmail,
                Data = $"Link to confirm: https://localhost:7054/api/Authorize/Confirm/{user.IdUser}?token={user.ConfirmToken}",
            };

            var emailContent = new StringContent(JsonConvert.SerializeObject(email), Encoding.UTF8, "application/json");
            // Query to Email Service for create UserToSend
            var response = await _httpClient.PostAsync("https://localhost:5004/api/Email", emailContent);
        
            return response;
        }

        public async Task<User?> CreateUserAsync(User user)
        {
            var userContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            // Query to User Service for create user
            var response = await _httpClient.PostAsync("https://localhost:5001/api/User", userContent);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var newUser = JsonConvert.DeserializeObject<User>(responseContent);

                return newUser;
            }

            return null;
        }

        public async Task<HttpResponseMessage> GetUserAsync(string username)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7000/api/User?userName={username}");
            
            return response;
        }
    }
}
