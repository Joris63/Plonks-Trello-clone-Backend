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
            modelBuilder.Entity<Board>()
                .HasMany(x => x.Members)
                .WithMany(x => x.Boards)
                .UsingEntity<BoardUsers>(
                    x => x.HasOne(x => x.User)
                    .WithMany().HasForeignKey(x => x.UserId),
                    x => x.HasOne(x => x.Board)
                   .WithMany().HasForeignKey(x => x.BoardId));

        }

        public DbSet<Board> Boards { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<BoardUsers> BoardUsers { get; set; }
    }
}
