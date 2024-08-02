using LikeMicroService.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace LikeMicroService.Core
{
    public class LikeMicroServiceContext : DbContext
    {
        public LikeMicroServiceContext(DbContextOptions<LikeMicroServiceContext> options) : base(options) { }

        public DbSet<Like> Likes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }


}
