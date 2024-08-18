using AuthorizeMicroService.Core.Models;

namespace AuthorizeMicroService.Application.Services
{
    public interface IAuthorizeService
    {
        public Task<User?> GetUserAsync(string login);
        public Task CreateUserAsync(User user);

        public bool VerifyUser(UserLogin userLogin, User user);
        
        public User BuildUser(UserUpload upload);
        public Task<bool> Confirm(int userId, string token);
    }
}
