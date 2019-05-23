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

        public const int MAP_WIDTH = 10;
        public const int MAP_HEIGHT = 10;
        public const int ACCESSIBLE_TILES = 10;

        public Game CreateNewGame(User player)
        {
            var game = new Game
            {
                Id = Guid.NewGuid().ToString(),
                Players = new List<User> { player },
                Tiles = MapGeneratorService.GenerateMap(MAP_WIDTH, MAP_HEIGHT, ACCESSIBLE_TILES).ToList()
            };
            Add(game);
            return Get(game.Id);
        }

        public Game GetOpenGame()
        {
            var games = Find(g => g.Players.Count < 2);
            if (games.Count() < 1) return default(Game);
            return games.First();
        }

        public Game JoinGame(Game game, User player)
        {
            game.Players.Add(player);
            game.NextMovePlayer = player;
            game.Tiles = MapGeneratorService.SetStartPostions(game.Tiles, game.Players).ToList();
            return game;
        }

        public Game RunningGameForPlayer(User player)
        {
            return SingleOrDefault(g => g.Players.Contains(player) && string.IsNullOrEmpty(g.WinningPlayerId));
        }
    }
}
