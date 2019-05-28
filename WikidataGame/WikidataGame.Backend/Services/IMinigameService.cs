using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WikidataGame.Backend.Dto;

namespace WikidataGame.Backend.Services
{
    public interface IMinigameService
    {
        Models.MiniGameType MiniGameType { get; }
        MiniGame GenerateMiniGame(string gameId, string playerId, string categoryId);
    }
}
