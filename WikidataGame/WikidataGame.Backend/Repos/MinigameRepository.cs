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

        public async Task<MiniGame> CreateMiniGameAsync(Guid gameId, Guid playerId, Guid tileId, Question question, MiniGameType type)
        {
            var minigame = new MiniGame
            {
                GameId = gameId,
                PlayerId = playerId,
                TileId = tileId,
                CategoryId = question.CategoryId,
                Type = type,
                QuestionId = question.Id
            };
            await AddAsync(minigame);
            return minigame;
        }
    }
}
