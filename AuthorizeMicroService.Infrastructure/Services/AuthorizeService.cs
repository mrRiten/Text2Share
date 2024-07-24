using AuthorizeMicroService.Application.Services;
using Newtonsoft.Json;
using AuthorizeMicroService.Core.Models;

namespace AuthorizeMicroService.Infrastructure.Services
{
    public class AuthorizeService() : IAuthorizeService
    {
        public string BuildUser(UserUpload upload)
        {
            var user = new User
            {
                UserName = upload.UserName,
                UserEmail = upload.UserEmail,
                Password = BCrypt.Net.BCrypt.HashPassword(upload.Password),

                DateOfRegister = DateTime.Now,
                LastLoginDate = DateTime.Now,
            };

            return JsonConvert.SerializeObject(user);
        }

        public bool VerifyUser(UserLogin userLogin, string userJson)
        {
            var user = JsonConvert.DeserializeObject<User>(userJson);

            if (user == null || !BCrypt.Net.BCrypt.Verify(userLogin.Password, user.Password)) { return false; }

            return true;
        }

    }
}
