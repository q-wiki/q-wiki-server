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
    public static Dictionary<(int, int), Models.Tile> GetNeighbors(IEnumerable<Models.Tile> tiles, int x, int y, int width, int height)
    {
        // odd and even tiles behave differently in a hexagonal grid; in our
        // grid, the [0, 0] sits to the top left of [1, 0] and right above [0, 1]
        var coordinates = x % 2 == 0
            ? new List<(int, int)> {
                (x - 1, y - 1),
                (x, y - 1),
                (x + 1, y - 1),
                (x - 1, y),
                (x + 1, y),
                (x, y + 1)
            }
            : new List<(int, int)> {
                (x, y - 1),
                (x - 1, y),
                (x + 1, y),
                (x - 1, y + 1),
                (x, y + 1),
                (x + 1, y + 1)
            };

        return coordinates
            .Where(((int x, int y) coord) => -1 < coord.x && coord.x < width && -1 < coord.y && coord.y < height)
            .ToDictionary(
                coordinate => coordinate,
                ((int x, int y) coord) => tiles.ElementAt(coord.x + coord.y * width)
            );
    }

    public static bool HasIslands (IEnumerable<Models.Tile> tiles, int width, int height) 
    {
        var colors = new Dictionary<(int, int), int>(); // maps a tuple of (x, y) to chosen colors
        var color = -1;
        var synonymousColors = new Dictionary<int, int>();

        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                var tile = tiles.ElementAt(x + y * width);
                if (tile.IsAccessible)
                {
                    var neighborCoords = GetNeighbors(tiles, x, y, width, height)
                        .Keys
                        .Where(((int x, int y) coord) => coord.x <= x && coord.y <= y)
                        .ToList();

                    // find a neighbor that has a color and use that
                    neighborCoords
                        .ForEach(coord => {
                            if (colors.ContainsKey(coord)) {
                                colors[(x, y)] = colors[coord];
                            } else {
                                color++;
                                colors[(x, y)] = color;
                            }
                        });
                    
                    // if neighboring fields are colored in more than one way,
                    // the colors are equivalent
                    neighborCoords
                        .Where(coord => colors.ContainsKey(coord))
                        .Select(coord => colors[coord])
                        .ToList()
                        .ForEach(neighborsColor => synonymousColors[neighborsColor] = color);
                }
            }
        }

        // remove all ambiguity by giving synonmous colors the same color
        while (synonymousColors.Count() > 0) 
        {
            (var fromColor, var toColor) = synonymousColors.First();
            colors.Keys.ToList().ForEach(coord => {
                if (colors[coord] == fromColor) {
                    colors[coord] = toColor;
                }
            });
            synonymousColors.Remove(fromColor);
        }

        return colors.Values.Distinct().Count() > 1;
    }
  }
}
