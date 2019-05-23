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

        public Tile[,] Tiles { get; set; }

        public Player Me { get; set; }

        public Player Opponent { get; set; }

        public string NextMovePlayerId { get; set; }

        public bool AwaitingOpponentToJoin { get; set; }

        public static Game FromModel(Models.Game game, string currentUserId)
        {
            if (game == null)
                return null;

            // convert tiles to two-dimensional array
            // TODO: This could probably be more idiomatic
            var tiles = game.Tiles.Select(t => Tile.FromModel(t)).AsEnumerable();
            var tileArray = new Tile[GameConstants.MAP_WIDTH, GameConstants.MAP_HEIGHT];
            for (var idx = 0; idx < tiles.Count(); idx++) {
                var x = idx % GameConstants.MAP_WIDTH;
                var y = idx - x / GameConstants.MAP_WIDTH;
                tileArray[x,y] = tiles.ElementAt(idx);
            }

            return new Game {
                Id = game.Id,
                Tiles = tileArray,
                AwaitingOpponentToJoin = game.Players.Count < 2,
                NextMovePlayerId = game.NextMovePlayerId,
                Me = Player.FromModel(game.Players.SingleOrDefault(p => p.DeviceId == currentUserId)),
                Opponent = Player.FromModel(game.Players.SingleOrDefault(p => p.DeviceId != currentUserId))
            };
        }
    }
}
