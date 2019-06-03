using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WikidataGame.Backend.Dto;

namespace WikidataGame.Backend.Helpers
{
    public static class TileHelper
    {
        public static IEnumerable<IEnumerable<Tile>> TileEnumerableModel2Dto(IEnumerable<Models.Tile> tiles)
        {
            return Enumerable.Range(0, GameConstants.MapHeight)
                .Select(yCoord =>
                    tiles.Skip(yCoord * GameConstants.MapWidth)
                        .Take(GameConstants.MapWidth)
                        // inaccessible tiles are represented as `null`
                        .Select(t => t.IsAccessible ? Tile.FromModel(t) : null)
                );
        }

        /// <summary>
        /// Returns 3 categories for a tile. The categories that are returned are
        /// stable and depend on the tile id.
        /// </summary>
        /// <param name="categoryRepo"></param>
        /// <param name="tile"></param>
        /// <returns></returns>
        public static IEnumerable<Models.Category> GetCategoriesForTile(Repos.IRepository<Models.Category, string> categoryRepo, Models.Tile tile)
        {
            // we get all categories, draw 3 distinct random ints in
            // [i, categories.Count()[ and return the categories for 
            // these draws
            var rnd = new Random(tile.Id.GetHashCode());
            var categories = categoryRepo.GetAll();

            var draws = new HashSet<Models.Category>();
            while (draws.Count() < 3)
            {
                var pick = rnd.Next(categories.Count());
                draws.Add(categories.ElementAt(pick));
            }

            return draws;
        }


    public static Dictionary<Tuple<int, int>, Models.Tile> GetNeighbors(IEnumerable<Models.Tile> tiles, int x, int y) {
        return new Dictionary<Tuple<int, int>, Models.Tile>();
    }
  }
}
