using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WikidataGame.Backend.Helpers;
using WikidataGame.Backend.Models;
using WikidataGame.Backend.Services;

namespace WikidataGame.Backend.Repos
{
    public class GameRepository : Repository<Game, string>, IGameRepository
    {
        public GameRepository(DataContext context) : base(context) { }

        public Game CreateNewGame(User player, int mapWidth, int mapHeight, int accessibleTiles)
        {
            var game = new Game
            {
                Id = Guid.NewGuid().ToString(),
                Tiles = MapGeneratorService.GenerateMap(mapWidth, mapHeight, accessibleTiles).ToList(),
                MapWidth = mapWidth,
                MapHeight = mapHeight,
                AccessibleTilesCount = accessibleTiles,
                StepsLeftWithinMove = Game.StepsPerPlayer,
                MoveCount = 0
            };
            var gameUser = new GameUser
            {
                GameId = game.Id,
                UserId = player.Id
            };

            game.GameUsers.Add(gameUser);
            Add(game);
            return Get(game.Id);
        }

        public Game GetOpenGame()
        {
            var games = Find(g => g.GameUsers.Count() < 2);
            if (games.Count() < 1) return null;
            return games.First();
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

        public Game RunningGameForPlayer(User player)
        {
            return SingleOrDefault(g => g.GameUsers.Select(gu => gu.UserId).Contains(player.Id) && g.GameUsers.Count(gu => gu.IsWinner) <= 0);
        }
    }
}
