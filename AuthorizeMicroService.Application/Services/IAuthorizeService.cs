using AuthorizeMicroService.Core.Models;

namespace AuthorizeMicroService.Application.Services
{
    public interface IAuthorizeService
    {
        public bool VerifyUser(UserLogin userLogin, string userJson);
        public string BuildUser(UserUpload upload);
    }
}
