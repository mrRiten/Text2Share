using EmailMicroService.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace EmailMicroService.Core
{
    public class EmailMicroServiceContext : DbContext
    {
        public EmailMicroServiceContext(DbContextOptions<EmailMicroServiceContext> options) : base(options) { }

        public DbSet<Email> Emails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
