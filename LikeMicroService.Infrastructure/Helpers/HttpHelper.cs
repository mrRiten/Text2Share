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
        private readonly HttpClient _httpClient = httpClient;
        private readonly XSource _source = source.Value;

        public async Task<HttpResponseMessage?> EditTextLikeAsync(LikeAction likeAction, int textId)
        {
            // Создание запроса с методом Get
            var request = new HttpRequestMessage(new HttpMethod("GET"), $"https://localhost:7000/api/Text/{textId}");

            // Добавление кастомного заголовка
            request.Headers.Add("X-Source", _source.Token);

            // Отправка запроса
            var response = await _httpClient.SendAsync(request);

            // Получение текста
            //var response = await _httpClient.GetAsync($"https://localhost:7000/api/Text/{textId}");

            if (!response.IsSuccessStatusCode) { return null; }

            var textJson = await response.Content.ReadAsStringAsync();
            var text = JsonConvert.DeserializeObject<Text>(textJson);

            // Подготовка данных JSON Patch
            var patchData = new PatchRequest[1];

            if (likeAction == LikeAction.Plus)
            {
                patchData[0] = new PatchRequest { Op = "replace", Path = "/LikeCount", Value = $"{text.LikeCount + 1}" };
            }
            else if (likeAction == LikeAction.Minus)
            {
                patchData[0] = new PatchRequest { Op = "replace", Path = "/LikeCount", Value = $"{text.LikeCount - 1}" };
            }

            var jsonPatch = JsonConvert.SerializeObject(patchData);
            var content = new StringContent(jsonPatch, Encoding.UTF8, "application/json-patch+json");

            // Создание запроса с методом PATCH
            request = new HttpRequestMessage(new HttpMethod("PATCH"), $"https://localhost:7000/api/Text/{textId}")
            {
                Content = content
            };

            // Добавление кастомного заголовка
            request.Headers.Add("X-Source", _source.Token);

            // Отправка запроса
            response = await _httpClient.SendAsync(request);

            return response;
        }


        public async Task<HttpResponseMessage> GetUserAsync(string userName)
        {
            return await _httpClient.GetAsync($"https://localhost:7000/api/User?userName={userName}");
        }

        public class PatchRequest
        {
            public string Op { get; set; }
            public string Path { get; set; }
            public string Value { get; set; }
        }
    }
}
