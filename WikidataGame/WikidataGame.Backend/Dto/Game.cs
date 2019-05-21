using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WikidataGame.Backend.Dto
{
    public class Game
    {
        public string Id { get; set; }

        public IEnumerable<Tile> Tiles { get; set; }

        public Player Me { get; set; }

        public Player Opponent { get; set; }

        public string NextMovePlayerId { get; set; }

        public bool AwaitingOpponentToJoin { get; set; }

        public static Game FromModel(Models.Game game, string currentUserId)
        {
            if (game == null)
                return null;

            return new Game
            {
                Id = game.Id,
                Tiles = game.Tiles.Select(t => Tile.FromModel(t)).AsEnumerable(),
                AwaitingOpponentToJoin = game.Players.Count < 2,
                NextMovePlayerId = game.NextMovePlayerId,
                Me = Player.FromModel(game.Players.SingleOrDefault(p => p.DeviceId == currentUserId)),
                Opponent = Player.FromModel(game.Players.SingleOrDefault(p => p.DeviceId != currentUserId))
            };
        }
    }
}
