using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WikidataGame.Backend.Models;

namespace WikidataGame.Backend.Repos
{
    public interface IGameRepository : IRepository<Game, string>
    {
        Task<Game> CreateNewGameAsync(User player, int mapWidth, int mapHeight, int accessibleTiles);

        Task<Game> GetOpenGameAsync();

        Game JoinGame(Game game, User player);

        Task<Game> RunningGameForPlayerAsync(User player);
    }
}
