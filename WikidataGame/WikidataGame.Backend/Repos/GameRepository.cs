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
    }
}
