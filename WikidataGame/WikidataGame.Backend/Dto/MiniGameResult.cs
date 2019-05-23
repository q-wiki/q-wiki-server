using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WikidataGame.Backend.Dto
{
    public class MiniGameResult
    {
        public bool IsWin { get; set; }

        public IEnumerable<string> CorrectAnswer { get; set; }

        public IEnumerable<Tile> Tiles { get; set; }

        public string NextMovePlayerId { get; set; }

        public static MiniGameResult FromModel(Models.MiniGame miniGame, Models.Game game)
        {
            return new MiniGameResult
            {
                IsWin = miniGame.IsWin,
                CorrectAnswer = miniGame.CorrectAnswer,
                Tiles = game.Tiles.Select(t => Tile.FromModel(t)).AsEnumerable(),
                NextMovePlayerId = game.NextMovePlayerId
            };
        }
    }
}
