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
    }
}
