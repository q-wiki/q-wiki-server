using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WikidataGame.Backend.Models;

namespace WikidataGame.Backend.Services
{
    public interface IMinigameService
    {
        MiniGameType MiniGameType { get; }
        Task<MiniGame> GenerateMiniGameAsync(Guid gameId, Guid playerId, Question category, Guid tileId);
    }
}
