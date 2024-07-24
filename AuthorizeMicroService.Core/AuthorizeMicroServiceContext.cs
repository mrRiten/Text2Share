using Microsoft.EntityFrameworkCore;
using AuthorizeMicroService.Core.Models;

namespace AuthorizeMicroService.Core
{
    public class AuthorizeMicroServiceContext : DbContext
    {
        public AuthorizeMicroServiceContext(DbContextOptions<AuthorizeMicroServiceContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
