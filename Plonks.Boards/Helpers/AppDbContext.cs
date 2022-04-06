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
            modelBuilder.Entity<BoardUsers>().HasKey(b => new { b.BoardId, b.UserId });
            modelBuilder.Entity<BoardUsers>()
                .HasOne(b => b.Board)
                .WithMany(b => b.Members)
                .HasForeignKey(uf => uf.BoardId);
            modelBuilder.Entity<BoardUsers>()
                .HasOne(b => b.User)
                .WithMany(b => b.Boards)
                .HasForeignKey(uf => uf.UserId);

        }

        public DbSet<Board> Boards { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<BoardUsers> BoardUsers { get; set; }
    }
}
