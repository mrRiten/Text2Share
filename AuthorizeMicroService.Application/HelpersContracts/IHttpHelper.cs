using AuthorizeMicroService.Core.Models;

namespace AuthorizeMicroService.Application.Helpers
{
    public interface IHttpHelper
    {
        public Task<HttpResponseMessage> CreateEmailAsync(User user);
    }
}
