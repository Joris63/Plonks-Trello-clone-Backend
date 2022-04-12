using Microsoft.EntityFrameworkCore;
using Plonks.Cards.Entities;

namespace Plonks.Cards.Helpers
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // MANY TO MANY
            // card - user relation
            modelBuilder.Entity<Card>()
                .HasMany(x => x.Users)
                .WithMany(x => x.Tasks)
                .UsingEntity<CardUsers>(
                    x => x.HasOne(x => x.User)
                    .WithMany().HasForeignKey(x => x.UserId),
                    x => x.HasOne(x => x.Card)
                   .WithMany().HasForeignKey(x => x.CardId));


            // ONE TO MANY
            // card - list relation
            modelBuilder.Entity<Card>()
                .HasOne(x => x.List)
                .WithMany(x => x.Cards)
                .HasForeignKey(x => x.ListId);

            // card - checklist relation
            modelBuilder.Entity<Checklist>()
                .HasOne(x => x.Card)
                .WithMany(x => x.Checklists)
                .HasForeignKey(x => x.CardId);

            // checklist - checklistItems relation
            modelBuilder.Entity<ChecklistItem>()
                .HasOne(x => x.Checklist)
                .WithMany(x => x.Items)
                .HasForeignKey(x => x.ChecklistId);

            // card - comments relation
            modelBuilder.Entity<Comment>()
                .HasOne(x => x.Card)
                .WithMany(x => x.Comments)
                .HasForeignKey(x => x.CardId);

            // comments - user relation
            modelBuilder.Entity<Comment>()
                .HasOne(x => x.Sender)
                .WithMany(x => x.Comments)
                .HasForeignKey(x => x.SenderId);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<BoardList> Lists { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<CardUsers> CardUsers { get; set; }
        public DbSet<Checklist> Checklists { get; set; }
        public DbSet<ChecklistItem> ChecklistItems { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}
