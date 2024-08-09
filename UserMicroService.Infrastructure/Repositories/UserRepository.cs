using Microsoft.EntityFrameworkCore;
using UserMicroService.Application.Repositories;
using UserMicroService.Core;
using UserMicroService.Core.Models;

namespace UserMicroService.Infrastructure.Repositories
{
    public class UserRepository(UserMicroServiceContext context) : IUserRepository
    {
        private readonly UserMicroServiceContext _context = context;

        public async Task CreateAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetAsync(string userName)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.UserName == userName);
        }

        public async Task<User?> GetAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
