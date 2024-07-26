using AuthorizeMicroService.Core.Models;

namespace AuthorizeMicroService.Application.Helpers
{
    public interface IHttpHelper
    {
        public Task<User?> CreateUserAsync(User user);
        public Task<HttpResponseMessage> CreateEmailAsync(User user);
    }
}
