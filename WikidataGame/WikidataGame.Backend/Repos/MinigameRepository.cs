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

        public async Task<bool> IsUserMinigamePlayerAsync(User user, Guid gameId, Guid minigameId)
        {
            var minigame = await GetAsync(minigameId);
            return minigame != null && minigame.GameId == gameId && minigame.Player == user;
        }

        public async Task<bool> HasPlayerAnOpenMinigameAsync(User user, Guid gameId)
        {
            return await SingleOrDefaultAsync(m => m.PlayerId == user.Id && m.GameId == gameId && m.Status == Models.MiniGameStatus.Unknown) != null;
        }
    }
}
