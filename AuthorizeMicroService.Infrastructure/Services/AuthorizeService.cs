using AuthorizeMicroService.Application.Services;
using Newtonsoft.Json;
using AuthorizeMicroService.Core.Models;
using AuthorizeMicroService.Core;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace AuthorizeMicroService.Infrastructure.Services
{
    public class AuthorizeService(AuthorizeMicroServiceContext context) : IAuthorizeService
    {
        private readonly AuthorizeMicroServiceContext _context = context;

        public User BuildUser(UserUpload upload)
        {
            byte[] randomBytes = new byte[32 / 2];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }

            var user = new User
            {
                UserName = upload.UserName,
                UserEmail = upload.UserEmail,
                Password = BCrypt.Net.BCrypt.HashPassword(upload.Password),
                ConfirmToken = BitConverter.ToString(randomBytes).Replace("-", ""),

                DateOfRegister = DateTime.Now,
                LastLoginDate = DateTime.Now,
            };

            return user;
        }

        public async Task<bool> Confirm(int userId, string token)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.IdUser == userId && u.ConfirmToken == token);

            if (user == null) { return false; }

            user.IsEmailConfirmed = true;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<User?> GetUserAsync(string login)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserName == login ||
                u.UserEmail == login);
        }

        public async Task CreateUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public bool VerifyUser(UserLogin userLogin, User user)
        {
            if (user == null || !BCrypt.Net.BCrypt.Verify(userLogin.Password, user.Password)) { return false; }

            user.LastLoginDate = DateTime.Now;
            _context.Users.Update(user);
            _context.SaveChanges();

            return true;
        }

    }
}
