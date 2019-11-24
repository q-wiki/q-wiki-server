using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WikidataGame.Backend.Models;

namespace WikidataGame.Backend.Helpers
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<MiniGame> MiniGames { get; set; }
        public DbSet<Tile> Tiles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Question> Questions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GameUser>().HasKey(gu => new { gu.GameId, gu.UserId });
            modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
            modelBuilder.Entity<User>().HasIndex(u => u.FirebaseUserId).IsUnique();

            base.OnModelCreating(modelBuilder);

            //Seed Database
            DatabaseSeeds.SeedCategories(modelBuilder);
            DatabaseSeeds.SeedQuestions(modelBuilder);
        }

    }
}
