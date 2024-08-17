﻿using AuthorizeMicroService.Application.Helpers;
using AuthorizeMicroService.Core.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;

namespace AuthorizeMicroService.Infrastructure.Helpers
{
    public class HttpHelper(HttpClient httpClient, IOptions<XSource> options) : IHttpHelper
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly XSource _source = options.Value;

        public async Task<HttpResponseMessage> CreateEmailAsync(User user)
        {
            var email = new Email
            {
                UserEmail = user.UserEmail,
                Data = $"Link to confirm: https://localhost:7003/api/Authorize/Confirm/{user.IdUser}?token={user.ConfirmToken}",
            };

            var emailContent = new StringContent(JsonConvert.SerializeObject(email), Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7000/api/Email")
            {
                Content = emailContent
            };
            request.Headers.Add("X-Source", _source.Token);

            var response = await _httpClient.SendAsync(request);

            return response;
        }

        public async Task<User?> CreateUserAsync(User user)
        {
            var userContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            // Query to User Service for create user
            var response = await _httpClient.PostAsync("https://localhost:7000/api/User", userContent);

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
            var request = new HttpRequestMessage(new HttpMethod("GET"), $"https://localhost:7000/api/User?userName={username}");
            request.Headers.Add("X-Source", _source.Token);

            var response = await _httpClient.SendAsync(request);
            
            return response;
        }
    }
}
