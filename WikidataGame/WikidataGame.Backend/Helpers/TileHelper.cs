using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WikidataGame.Backend.Models;
using WikidataGame.Backend.Repos;
using WikidataGame.Backend.Services;

namespace WikidataGame.Backend.Helpers
{
    public static class TileHelper
    {
        public static IEnumerable<IEnumerable<Dto.Tile>> TileEnumerableModel2Dto(Game game, CategoryCacheService categoryCacheService)
        {
            return Enumerable.Range(0, game.MapHeight)
                .Select(yCoord =>
                    game.Tiles.Skip(yCoord * game.MapWidth)
                        .Take(game.MapWidth)
                        // inaccessible tiles are represented as `null`
                        .Select(async t => t.IsAccessible ? await Dto.Tile.FromModelAsync(t, categoryCacheService) : null)
                        .Select(t => t.Result)
                );
        }

        /// <summary>
        /// Returns 3 categories for a tile. The categories that are returned are
        /// stable and depend on the tile id.
        /// </summary>
        /// <param name="categoryService"></param>
        /// <param name="tileId"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<Category>> GetCategoriesForTileAsync(CategoryCacheService categoryService, Guid tileId)
        {
            // we get all categories, draw 3 distinct random ints in
            // [i, categories.Count()[ and return the categories for
            // these draws
            var rnd = new Random(tileId.GetHashCode());
            var categories = await categoryService.GetCategoriesAsync();

            var draws = new HashSet<Category>();
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
        public static Dictionary<(int, int), Tile> GetNeighbors(IEnumerable<Tile> tiles, int x, int y, int width, int height)
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

        public static bool HasIslands (IEnumerable<Tile> tiles, int width, int height)
        {
            var colors = new Dictionary<(int, int), int?>(); // maps a tuple of (x, y) to chosen colors
            var color = -1;
            var synonymousColors = new Dictionary<int, int>();

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var tile = tiles.ElementAt(x + y * width);
                    if (tile.IsAccessible)
                    {
                        var neighborCoords = GetNeighbors(tiles, x, y, width, height)
                            .Keys
                            .Where(((int _x, int _y) coord) => coord._x <= x && coord._y <= y)
                            .ToList();

                        // try to find a neighbor that has a color and use that
                        var neighborColor = neighborCoords
                            .Select(neighbor => colors.GetValueOrDefault(neighbor, null))
                            .FirstOrDefault(c => c != null);

                        if(neighborColor != null)
                        {
                            colors[(x, y)] = neighborColor;
                        }
                        else
                        {
                            colors[(x, y)] = ++color;
                        }

                        // if neighboring fields use a different color,
                        // the colors are equivalent
                        neighborCoords
                            .Select(coord => colors.GetValueOrDefault(coord, color))
                            .Where(neighboringColor => neighboringColor != color)
                            .ToList()
                            .ForEach(neighboringColor => synonymousColors[neighboringColor.Value] = color);
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
