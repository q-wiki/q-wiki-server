using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WikidataGame.Backend.Models;

namespace WikidataGame.Backend.Repos
{
    public interface IGameRepository : IRepository<Game, string>
    {
        Game CreateNewGame(User player);

        Game GetOpenGame();

        Game JoinGame(Game game, User player);

        Game RunningGameForPlayer(User player);
    }
}
