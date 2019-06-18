using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WikidataGame.Backend.Helpers;
using WikidataGame.Backend.Models;

namespace WikidataGame.Backend.Dto
{
    public class Game
    {
        public string Id { get; set; }

        public IEnumerable<IEnumerable<Tile>> Tiles { get; set; }

        public IEnumerable<string> WinningPlayerIds { get; set; }

        public Player Me { get; set; }

        public Player Opponent { get; set; }

        public string NextMovePlayerId { get; set; }

        public bool AwaitingOpponentToJoin { get; set; }

        public static Game FromModel(Models.Game game, string currentUserId, Repos.IRepository<Models.Category, string> categoryRepo)
        {
            if (game == null)
                return null;

            return new Game {
                Id = game.Id,
                Tiles = TileHelper.TileEnumerableModel2Dto(game, categoryRepo),
                AwaitingOpponentToJoin = game.GameUsers.Count() < 2,
                WinningPlayerIds = game.GameUsers.Where(gu => gu.IsWinner).Select(gu => gu.UserId).ToList(),
                NextMovePlayerId = game.NextMovePlayerId,
                Me = Player.FromModel(game.GameUsers.SingleOrDefault(gu => gu.UserId == currentUserId)?.User),
                Opponent = Player.FromModel(game.GameUsers.SingleOrDefault(gu => gu.UserId != currentUserId)?.User)
            };
        }
    }
}
