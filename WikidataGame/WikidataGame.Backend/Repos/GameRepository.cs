using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WikidataGame.Backend.Helpers;
using WikidataGame.Backend.Models;
using WikidataGame.Backend.Services;

namespace WikidataGame.Backend.Repos
{
    public class GameRepository : Repository<Game, Guid>, IGameRepository
    {
        public GameRepository(DataContext context) : base(context) { }

        public async Task<Game> CreateNewGameAsync(User player)
        {
            var game = new Game
            {
                Tiles = MapGeneratorService.GenerateMap(GameConstants.DefaultMapWidth, GameConstants.DefaultMapHeight, GameConstants.DefaultAccessibleTilesCount).ToList(),
                MapWidth = GameConstants.DefaultMapWidth,
                MapHeight = GameConstants.DefaultMapHeight,
                AccessibleTilesCount = GameConstants.DefaultAccessibleTilesCount,
                StepsLeftWithinMove = Game.StepsPerPlayer,
                MoveCount = 0
            };
            var gameUser = new GameUser
            {
                GameId = game.Id,
                UserId = player.Id
            };

            game.GameUsers.Add(gameUser);
            await AddAsync(game);
            return await GetAsync(game.Id);
        }

        public async Task<IEnumerable<Game>> GetOpenGamesAsync()
        {
            return await FindAsync(g => g.GameUsers.Count() < 2);
        }

        public async Task<IEnumerable<Game>> GetGamesForUserToJoinAsync(User user)
        {
            var runningGames = await RunningGamesForPlayerAsync(user);
            var runningGameOpponentIds = runningGames.Select(g => g.GameUsers.SingleOrDefault(gu => gu.UserId != user.Id)?.UserId).ToList();
            return await FindAsync(g => g.GameUsers.Count() < 2 && 
                ! g.GameUsers.Any(gu => gu.UserId == user.Id) &&
                ! runningGameOpponentIds.Contains(g.GameUsers.SingleOrDefault(gu => gu.UserId != user.Id).UserId));
        }

        public async Task<IEnumerable<Game>> GetOpenGamesForUserAsync(User user)
        {
            return await FindAsync(g => g.GameUsers.Count() < 2 && g.GameUsers.SingleOrDefault(gu => gu.UserId == user.Id) != null);
        }

        public Game JoinGame(Game game, User player)
        {
            game.GameUsers.Add(new GameUser
            {
                GameId = game.Id,
                UserId = player.Id
            });
            game.NextMovePlayerId = player.Id;
            game.MoveStartedAt = DateTime.UtcNow;
            game.Tiles = MapGeneratorService.SetStartPositions(game.Tiles, game.GameUsers.Select(gu => gu.UserId)).ToList();
            return game;
        }

        public async Task<IEnumerable<Game>> RunningGamesForPlayerAsync(User player)
        {
            return await FindAsync(g => g.GameUsers.Select(gu => gu.UserId).Contains(player.Id) && g.GameUsers.Count(gu => gu.IsWinner) <= 0);
        }

        public async Task<bool> IsUserGameParticipantAsync(User user, Guid gameId)
        {
            var game = await GetAsync(gameId);
            return game != null && game.GameUsers.SingleOrDefault(gu => gu.UserId == user.Id) != null;
        }

        public async Task<bool> IsItPlayersTurnAsync(User user, Guid gameId)
        {
            var game = await GetAsync(gameId);
            return game.NextMovePlayerId == user.Id;
        }

        public async Task<bool> IsTileInGameAsync(Guid gameId, Guid tileId)
        {
            var game = await GetAsync(gameId);
            return game.Tiles.SingleOrDefault(t => t.Id == tileId) != null;
        }

        public async Task<bool> IsCategoryAllowedForTileAsync(CategoryCacheService ccs, Guid gameId, Guid tileId, Guid categoryId)
        {
            var game = await GetAsync(gameId);
            var tile = game.Tiles.SingleOrDefault(t => t.Id == tileId);
            return (!tile.ChosenCategoryId.HasValue || tile.ChosenCategoryId == categoryId) &&
                TileHelper.GetCategoriesForTile(ccs, tileId).SingleOrDefault(c => c.Id == categoryId) != null;
        }

        public async Task<IEnumerable<Guid>> WinningPlayerIdsAsync(Guid gameId)
        {
            var game = await GetAsync(gameId);
            var result = game.GameUsers.ToDictionary(gu => gu.UserId, gu => 0);
            var tiles = game.Tiles.ToList();
            foreach (var tile in tiles)
            {
                if (tile.OwnerId.HasValue)
                {
                    result[tile.OwnerId.Value] += tile.Difficulty + 1;
                }
            }
            var rankedPlayers = result.OrderByDescending(r => r.Value);
            return rankedPlayers.Where(p => p.Value >= rankedPlayers.First().Value).Select(p => p.Key).ToList();
        }

        public async Task<bool> AllTilesConqueredAsync(User user, Guid gameId)
        {
            var game = await GetAsync(gameId);
            var opponentId = game.GameUsers.SingleOrDefault(gu => gu.UserId != user.Id).UserId;
            return game.Tiles.Count(t => t.OwnerId == opponentId) < 1;
        }

        public async Task SetGameWonAsync(Game game, INotificationService notificationService)
        {
            game.NextMovePlayerId = null;
            var winningPlayerIds = await WinningPlayerIdsAsync(game.Id);
            foreach (var winnerId in winningPlayerIds)
            {
                var user = game.GameUsers.SingleOrDefault(gu => gu.UserId == winnerId);
                user.IsWinner = true;
                await notificationService.SendNotificationAsync(
                    PushType.YouWon,
                    user.User,
                    game.GameUsers.SingleOrDefault(gu => gu.UserId != winnerId)?.User,
                    game.Id);
            }
            foreach (var looser in game.GameUsers.Where(gu => !winningPlayerIds.Contains(gu.UserId)))
            {
                await notificationService.SendNotificationAsync(
                    PushType.YouLost,
                    looser.User,
                    game.GameUsers.SingleOrDefault(gu => winningPlayerIds.Contains(gu.UserId))?.User,
                    game.Id);
            }
        }
    }
}
