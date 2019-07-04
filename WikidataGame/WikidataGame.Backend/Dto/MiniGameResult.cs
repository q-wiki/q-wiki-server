using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WikidataGame.Backend.Helpers;
using WikidataGame.Backend.Services;

namespace WikidataGame.Backend.Dto
{
    public class MiniGameResult
    {
        public bool IsWin { get; set; }

        public IEnumerable<string> CorrectAnswer { get; set; }

        public IEnumerable<IEnumerable<Tile>> Tiles { get; set; }

        public string NextMovePlayerId { get; set; }

        public static MiniGameResult FromModel(Models.MiniGame miniGame, Models.Game game, CategoryCacheService categoryCacheService)
        {
            return new MiniGameResult
            {
                IsWin = miniGame.Status == Models.MiniGameStatus.Win,
                CorrectAnswer = miniGame.CorrectAnswer,
                Tiles = TileHelper.TileEnumerableModel2Dto(game, categoryCacheService),
                NextMovePlayerId = game.NextMovePlayerId
            };
        }
    }
}
