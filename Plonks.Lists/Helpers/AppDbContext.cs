using Microsoft.EntityFrameworkCore;
using Plonks.Lists.Entities;

namespace Plonks.Lists.Helpers
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Card>()
                .HasMany(x => x.Users)
                .WithMany(x => x.Cards)
                .UsingEntity<CardUsers>(
                    x => x.HasOne(x => x.User)
                    .WithMany().HasForeignKey(x => x.UserId),
                    x => x.HasOne(x => x.Card)
                   .WithMany().HasForeignKey(x => x.CardId));
        }

        public DbSet<BoardList> Lists { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<CardUsers> CardUsers { get; set; }
    }
}
