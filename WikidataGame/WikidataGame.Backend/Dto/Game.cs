using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WikidataGame.Backend.Helpers;

namespace WikidataGame.Backend.Dto
{
    public class Game
    {
        public string Id { get; set; }

        public IEnumerable<IEnumerable<Tile>> Tiles { get; set; }

        public Player Me { get; set; }

        public Player Opponent { get; set; }

        public string NextMovePlayerId { get; set; }

        public bool AwaitingOpponentToJoin { get; set; }

        public static Game FromModel(Models.Game game, string currentUserId)
        {
            if (game == null)
                return null;

            // convert tiles to two-dimensional array
            var tiles = Enumerable.Range(0, GameConstants.MapHeight)
                .Select(yCoord =>
                    game.Tiles.Skip(yCoord * GameConstants.MapWidth)
                        .Take(GameConstants.MapWidth)
                        // inaccessible tiles are represented as `null`
                        .Select(t => t.IsAccessible ? Tile.FromModel(t) : null)
                );

            return new Game {
                Id = game.Id,
                Tiles = tiles,
                AwaitingOpponentToJoin = game.Players.Count < 2,
                NextMovePlayerId = game.NextMovePlayerId,
                Me = Player.FromModel(game.Players.SingleOrDefault(p => p.DeviceId == currentUserId)),
                Opponent = Player.FromModel(game.Players.SingleOrDefault(p => p.DeviceId != currentUserId))
            };
        }
    }
}
