using AuthorizeMicroService.Core.Models;

namespace AuthorizeMicroService.Application.Services
{
    public interface IAuthorizeService
    {
        public bool VerifyUser(UserLogin userLogin, string userJson);
        public User BuildUser(UserUpload upload);
        public Task<bool> Confirm(int userId, string token);
    }
}
