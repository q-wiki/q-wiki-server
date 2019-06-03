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


    /// <summary>
    /// Given an IEnumerable of Tiles in a grid of hexagonal tiles and some
    /// additional information about the map dimensions, returns all neighbors
    /// of the tile.
    /// </summary>
    /// <param name="tiles">One-dimensional list of tiles</param>
    /// <param name="x">X-Coordinate of the tile to get the neighbors for</param>
    /// <param name="y">Y-Coordinate of the tile to get the neighbors for</param>
    /// <param name="width">Map width</param>
    /// <param name="height">Map height</param>
    /// <returns>A dict of neighbors that maps coordinates to tiles.</returns>
    public static Dictionary<Tuple<int, int>, Models.Tile> GetNeighbors(IEnumerable<Models.Tile> tiles, int x, int y, int width, int height) {
        // odd and even tiles behave differently in a hexagonal grid; in our
        // grid, the [0, 0] sits to the top left of [1, 0] and right above [0, 1]
        var coordinates = x % 2 == 0
            ? new List<Tuple<int, int>> {
                Tuple.Create(x - 1, y - 1),                
                Tuple.Create(x, y - 1),                
                Tuple.Create(x + 1, y - 1),                
                Tuple.Create(x - 1, y),                
                Tuple.Create(x + 1, y),                
                Tuple.Create(x, y + 1)                
            }
            : new List<Tuple<int, int>> {
                Tuple.Create(x, y - 1),
                Tuple.Create(x - 1, y),
                Tuple.Create(x + 1, y),
                Tuple.Create(x - 1, y + 1),
                Tuple.Create(x, y + 1),
                Tuple.Create(x + 1, y + 1),
            };
        
        return coordinates
            .Where(coordinate => -1 < coordinate.Item1 && coordinate.Item1 < width && -1 < coordinate.Item2 && coordinate.Item2 < width)
            .ToDictionary(
                coordinate => coordinate,
                coordinate => tiles.ElementAt(coordinate.Item1 + coordinate.Item2 * width)
            );
    }
  }
}
