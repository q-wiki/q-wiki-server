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
    }
}
