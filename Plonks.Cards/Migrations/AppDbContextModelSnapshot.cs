﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Plonks.Cards.Helpers;

#nullable disable

namespace Plonks.Cards.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Plonks.Cards.Entities.BoardList", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Lists");
                });

            modelBuilder.Entity("Plonks.Cards.Entities.Card", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Archived")
                        .HasColumnType("bit");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("ListId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ListId");

                    b.ToTable("Cards");
                });

            modelBuilder.Entity("Plonks.Cards.Entities.CardUsers", b =>
                {
                    b.Property<Guid>("CardId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("CardId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("CardUsers");
                });

            modelBuilder.Entity("Plonks.Cards.Entities.Checklist", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CardId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CardId");

                    b.ToTable("Checklists");
                });

            modelBuilder.Entity("Plonks.Cards.Entities.ChecklistItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ChecklistId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Complete")
                        .HasColumnType("bit");

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ChecklistId");

                    b.ToTable("ChecklistItems");
                });

            modelBuilder.Entity("Plonks.Cards.Entities.Comment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CardId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Message")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("SenderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("SentAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("CardId");

                    b.HasIndex("SenderId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("Plonks.Cards.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PicturePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Plonks.Cards.Entities.Card", b =>
                {
                    b.HasOne("Plonks.Cards.Entities.BoardList", "List")
                        .WithMany("Cards")
                        .HasForeignKey("ListId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("List");
                });

            modelBuilder.Entity("Plonks.Cards.Entities.CardUsers", b =>
                {
                    b.HasOne("Plonks.Cards.Entities.Card", "Card")
                        .WithMany()
                        .HasForeignKey("CardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Plonks.Cards.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Card");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Plonks.Cards.Entities.Checklist", b =>
                {
                    b.HasOne("Plonks.Cards.Entities.Card", "Card")
                        .WithMany("Checklists")
                        .HasForeignKey("CardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Card");
                });

            modelBuilder.Entity("Plonks.Cards.Entities.ChecklistItem", b =>
                {
                    b.HasOne("Plonks.Cards.Entities.Checklist", "Checklist")
                        .WithMany("Items")
                        .HasForeignKey("ChecklistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Checklist");
                });

            modelBuilder.Entity("Plonks.Cards.Entities.Comment", b =>
                {
                    b.HasOne("Plonks.Cards.Entities.Card", "Card")
                        .WithMany("Comments")
                        .HasForeignKey("CardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Plonks.Cards.Entities.User", "Sender")
                        .WithMany("Comments")
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Card");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("Plonks.Cards.Entities.BoardList", b =>
                {
                    b.Navigation("Cards");
                });

            modelBuilder.Entity("Plonks.Cards.Entities.Card", b =>
                {
                    b.Navigation("Checklists");

                    b.Navigation("Comments");
                });

            modelBuilder.Entity("Plonks.Cards.Entities.Checklist", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("Plonks.Cards.Entities.User", b =>
                {
                    b.Navigation("Comments");
                });
#pragma warning restore 612, 618
        }
    }
}
