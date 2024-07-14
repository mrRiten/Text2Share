using UserMicroService.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace UserMicroService.Core
{
    public class UserMicroServiceContext : DbContext
    {
        public UserMicroServiceContext(DbContextOptions<UserMicroServiceContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
