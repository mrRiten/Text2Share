using EmailMicroService.Application.Repositories;
using EmailMicroService.Core;
using EmailMicroService.Core.Models;

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
    }
}
