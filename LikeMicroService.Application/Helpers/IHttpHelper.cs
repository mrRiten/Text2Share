﻿namespace LikeMicroService.Application.Helpers
{
    public interface IHttpHelper
    {
        public Task<HttpResponseMessage> GetUserAsync(string userName);
    }
}
