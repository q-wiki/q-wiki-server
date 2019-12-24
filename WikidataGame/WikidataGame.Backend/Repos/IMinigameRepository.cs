using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WikidataGame.Backend.Models;

namespace WikidataGame.Backend.Repos
{
    public interface IMinigameRepository : IRepository<MiniGame, Guid>
    {
        Task<MiniGame> CreateMiniGameAsync(Guid gameId, Guid playerId, Guid tileId, Question question, MiniGameType type);
        Task<bool> HasPlayerAnOpenMinigameAsync(User user, Guid gameId);
        Task<bool> IsUserMinigamePlayerAsync(User user, Guid gameId, Guid minigameId);
    }
}
