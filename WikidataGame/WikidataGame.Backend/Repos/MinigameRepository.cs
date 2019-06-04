using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WikidataGame.Backend.Helpers;
using WikidataGame.Backend.Models;

namespace WikidataGame.Backend.Repos
{
    public class MinigameRepository : Repository<MiniGame, string>, IMinigameRepository
    {
        public MinigameRepository(DataContext context) : base(context)
        {
        }

        public MiniGame CreateMiniGame(string gameId, string playerId, MiniGameType type)
        {
            var minigame = new MiniGame
            {
                Id = Guid.NewGuid().ToString(),
                GameId = gameId,
                PlayerId = playerId,
                Type = type,
                IsWin = false
            };
            Add(minigame);
            return minigame;
        }
    }
}
