using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WikidataGame.Backend.Models;

namespace WikidataGame.Backend.Repos
{
    public interface IMinigameRepository : IRepository<MiniGame, string>
    {
        MiniGame CreateMiniGame(string gameId, string playerId, string tileId, string categoryId, MiniGameType type);
    }
}
