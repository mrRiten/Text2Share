using EmailMicroService.Application.Repositories;
using EmailMicroService.Core;
using EmailMicroService.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace EmailMicroService.Infrastructure.Repositories
{
    public class EmailRepository(EmailMicroServiceContext context) : IEmailRepository
    {
        private readonly EmailMicroServiceContext _context = context;

        public async Task CreateAsync(Email email)
        {
            await _context.Emails.AddAsync(email);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Email[] emails)
        {
            _context.RemoveRange(emails);
            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<Email>> GetAllAsync()
        {
            return await _context.Emails.ToListAsync();
        }
    }
}
