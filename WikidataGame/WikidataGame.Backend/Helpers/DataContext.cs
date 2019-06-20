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
            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
               .SelectMany(t => t.GetForeignKeys())
               .Where(fk => !fk.IsOwnership && fk.DeleteBehavior != DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Cascade;

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<GameUser>().HasKey(table => new { table.GameId, table.UserId });
            modelBuilder.Entity<User>().HasAlternateKey(c => c.DeviceId);

            //Seed Database
            DatabaseSeeds.SeedCategories(modelBuilder);
            DatabaseSeeds.SeedQuestions(modelBuilder);
        }

    }
}
