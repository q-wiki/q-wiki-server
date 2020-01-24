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
        Task<bool> IsUserGameParticipantAsync(User user, Guid gameId);
        Task<bool> IsItPlayersTurnAsync(User user, Guid gameId);
        Task<bool> IsTileInGameAsync(Guid gameId, Guid tileId);
        Task<bool> IsCategoryAllowedForTileAsync(Services.CategoryCacheService ccs, Guid gameId, Guid tileId, Guid categoryId);
        Task<IEnumerable<Guid>> WinningPlayerIdsAsync(Guid gameId);
        Task<bool> AllTilesConqueredAsync(User user, Guid gameId);
        Task SetGameWonAsync(Game game, Services.INotificationService notificationService);
    }
}
