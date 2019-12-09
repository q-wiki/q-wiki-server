using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WikidataGame.Backend.Models;

namespace WikidataGame.Backend.Repos
{
    public interface IGameRepository : IRepository<Game, Guid>
    {
        Task<Game> CreateNewGameAsync(User player);

        Task<IEnumerable<Game>> GetOpenGamesAsync();

        Task<IEnumerable<Game>> GetGamesForUserToJoinAsync(User user);

        Game JoinGame(Game game, User player);

        Task<IEnumerable<Game>> RunningGamesForPlayerAsync(User player);
    }
}
