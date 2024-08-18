using LikeMicroService.Application.Helpers;
using LikeMicroService.Core.Enums;
using LikeMicroService.Core.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;

namespace LikeMicroService.Infrastructure.Helpers
{
    public class HttpHelper(HttpClient httpClient, IOptions<XSource> source) : IHttpHelper
    {
        private readonly HttpClient _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        private readonly XSource _source = source?.Value ?? throw new ArgumentNullException(nameof(source));

        public async Task<HttpResponseMessage?> EditTextLikeAsync(LikeAction likeAction, int textId)
        {
            // Create GET-request for get text
            var getRequest = new HttpRequestMessage(HttpMethod.Get, $"https://localhost:7000/api/Text/{textId}");
            getRequest.Headers.Add("X-Source", _source.Token);

            var response = await _httpClient.SendAsync(getRequest);
            if (!response.IsSuccessStatusCode) return null;

            var textJson = await response.Content.ReadAsStringAsync();
            var text = JsonConvert.DeserializeObject<Text>(textJson);
            if (text == null) return null;

            // Compose data JSON Patch
            var patchValue = likeAction == LikeAction.Plus ? text.LikeCount + 1 : text.LikeCount - 1;
            var patchData = new[]
            {
                new PatchRequest
                {
                    Op = "replace",
                    Path = "/LikeCount",
                    Value = patchValue.ToString()
                }
            };

            var jsonPatch = JsonConvert.SerializeObject(patchData);
            var patchContent = new StringContent(jsonPatch, Encoding.UTF8, "application/json-patch+json");

            // Create PATCH-request for update text
            var patchRequest = new HttpRequestMessage(HttpMethod.Patch, $"https://localhost:7000/api/Text/{textId}")
            {
                Content = patchContent
            };
            patchRequest.Headers.Add("X-Source", _source.Token);

            return await _httpClient.SendAsync(patchRequest);
        }

        public async Task<HttpResponseMessage> GetUserAsync(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentException("User name cannot be null or whitespace.", nameof(userName));
            }

            return await _httpClient.GetAsync($"https://localhost:7000/api/User?userName={userName}");
        }

        private class PatchRequest
        {
            public required string Op { get; set; }
            public required string Path { get; set; }
            public required string Value { get; set; }
        }
    }
}
