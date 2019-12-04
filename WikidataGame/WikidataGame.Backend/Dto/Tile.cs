using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WikidataGame.Backend.Helpers;
using WikidataGame.Backend.Repos;
using WikidataGame.Backend.Services;

namespace WikidataGame.Backend.Dto
{
    public class Tile
    {
        public Guid Id { get; set; }

        public Guid? ChosenCategoryId { get; set; }

        public IEnumerable<Category> AvailableCategories { get; set; }

        public int Difficulty { get; set; }

        public Guid? OwnerId { get; set; }

        public static async Task<Tile> FromModelAsync(Models.Tile tile, CategoryCacheService categoryService)
        {
            if (tile == null)
                return null;

            return new Tile
            {
                Id = tile.Id,
                ChosenCategoryId = tile.ChosenCategoryId,
                AvailableCategories = (await TileHelper.GetCategoriesForTileAsync(categoryService, tile.Id)).Select(c => Category.FromModel(c)).ToList(),
                Difficulty = tile.Difficulty,
                OwnerId = tile.OwnerId
            };
        }
    }
}
