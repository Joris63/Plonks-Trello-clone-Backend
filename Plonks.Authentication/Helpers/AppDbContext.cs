using Microsoft.EntityFrameworkCore;
using Plonks.Auth.Entities;

namespace Plonks.Auth.Helpers
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users");
        }

        public DbSet<User> Users { get; set; }
    }
}
