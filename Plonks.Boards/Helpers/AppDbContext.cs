using Microsoft.EntityFrameworkCore;
using Plonks.Boards.Entities;

namespace Plonks.Boards.Helpers
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }

        public DbSet<Board> Boards { get; set; }
    }
}
