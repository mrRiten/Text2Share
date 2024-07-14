using Microsoft.EntityFrameworkCore;
using TextMicroService.Core.Models;

namespace TextMicroService.Core
{
    public class TextMicroServiceContext : DbContext
    {
        public TextMicroServiceContext(DbContextOptions<TextMicroServiceContext> options) : base(options) { }

        public DbSet<Text> Texts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }
    }
}
