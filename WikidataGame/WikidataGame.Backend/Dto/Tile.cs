using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WikidataGame.Backend.Helpers;
using WikidataGame.Backend.Repos;

namespace WikidataGame.Backend.Dto
{
    public class Tile
    {
        public string Id { get; set; }

        public string ChosenCategoryId { get; set; }

        public IEnumerable<Category> AvailableCategories { get; set; }

        public int Difficulty { get; set; }

        public string OwnerId { get; set; }

        public static Tile FromModel(Models.Tile tile, IRepository<Models.Category, string> categoryRepo)
        {
            if (tile == null)
                return null;

            return new Tile
            {
                Id = tile.Id,
                ChosenCategoryId = tile.ChosenCategoryId,
                AvailableCategories = TileHelper.GetCategoriesForTile(categoryRepo, tile.Id).Select(c => Category.FromModel(c)),
                Difficulty = tile.Difficulty,
                OwnerId = tile.OwnerId
            };
        }
    }
}
