using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WikidataGame.Backend.Helpers;
using WikidataGame.Backend.Models;

namespace WikidataGame.Backend.Repos
{
    public class MinigameRepository : Repository<MiniGame, Guid>, IMinigameRepository
    {
        public MinigameRepository(DataContext context) : base(context)
        {
        }

        public async Task<MiniGame> CreateMiniGameAsync(Guid gameId, Guid playerId, Guid tileId, Guid categoryId, MiniGameType type)
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
