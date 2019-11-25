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

        public async Task<MiniGame> CreateMiniGameAsync(string gameId, string playerId, string tileId, string categoryId, MiniGameType type)
        {
            var minigame = new MiniGame
            {
                GameId = gameId,
                PlayerId = playerId,
                TileId = tileId,
                CategoryId = categoryId,
                Type = type,
            };
            await AddAsync(minigame);
            return minigame;
        }
    }
}
