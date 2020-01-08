using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WikidataGame.Backend.Models;

namespace WikidataGame.Backend.Helpers
{
    public class DataContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Friend> Friends { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<MiniGame> MiniGames { get; set; }
        public DbSet<Tile> Tiles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionRating> QuestionRatings { get; set; }
        public DbSet<GameRequest> GameRequests { get; set; }
        public DbSet<Report> Reports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GameUser>().HasKey(gu => new { gu.GameId, gu.UserId });
            modelBuilder.Entity<User>().HasIndex(u => u.UserName).IsUnique();
            modelBuilder.Entity<Friend>().HasKey(f => new { f.UserId, f.FriendId });

            modelBuilder.Entity<Friend>()
                .HasOne(f => f.User)
                .WithMany()
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Friend>()
                .HasOne(f => f.FriendUser)
                .WithMany()
                .HasForeignKey(f => f.FriendId);

            modelBuilder.Entity<GameRequest>().HasKey(gr => gr.Id);
            modelBuilder.Entity<GameRequest>()
                .HasOne(f => f.Sender)
                .WithMany()
                .HasForeignKey(f => f.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<GameRequest>()
                .HasOne(f => f.Recipient)
                .WithMany()
                .HasForeignKey(f => f.RecipientId);

            modelBuilder.Entity<MiniGame>()
                .HasOne(m => m.Question)
                .WithMany()
                .HasForeignKey(m => m.QuestionId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);

            //Seed Database
            DatabaseSeeds.SeedCategories(modelBuilder);
            DatabaseSeeds.SeedQuestions(modelBuilder);
        }

    }
}
