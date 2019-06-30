using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using WikidataGame.Backend.Dto;
using WikidataGame.Backend.Helpers;
using WikidataGame.Backend.Repos;
using WikidataGame.Backend.Services;

namespace WikidataGame.Backend.Controllers
{
    [Route("api/games/{gameId}/minigames")]
    [Authorize]
    [ApiController]
    public class MinigamesController : CustomControllerBase
    {
        private readonly IMinigameRepository _minigameRepo;

        public MinigamesController(
            DataContext dataContext,
            IUserRepository userRepo,
            IGameRepository gameRepo,
            IMinigameRepository minigameRepo,
            IRepository<Models.Category, string> categoryRepo,
            INotificationService notificationService) : base(dataContext, userRepo, gameRepo, categoryRepo, notificationService)
        {
            _minigameRepo = minigameRepo;
        }

        /// <summary>
        /// Initializes a new minigame
        /// </summary>
        /// <param name="gameId">game identifier</param>
        /// <param name="minigameParams">minigame information containing category and tile identifier</param>
        /// <returns>The created minigame</returns>
        [HttpPost]
        [ProducesResponseType(typeof(MiniGame), StatusCodes.Status200OK)]
        public IActionResult InitalizeMinigame(string gameId, MiniGameInit minigameParams)
        {
            if (!IsUserGameParticipant(gameId) || minigameParams == null ||
                !IsTileInGame(gameId, minigameParams.TileId) ||
                !IsCategoryAllowedForTile(gameId, minigameParams.TileId, minigameParams.CategoryId) ||
                !IsItPlayersTurn(gameId) ||
                HasPlayerAnOpenMinigame(gameId))
                    return Forbid();

            var minigameServices = ControllerContext.HttpContext.RequestServices.GetServices<IMinigameService>();
            var random = new Random();
            var randomService = minigameServices.ElementAt(random.Next(0, minigameServices.Count()));

            var minigame = randomService.GenerateMiniGame(gameId, GetCurrentUser().Id, minigameParams.CategoryId, minigameParams.TileId);

            return Ok(minigame);
        }

        /// <summary>
        /// Retrieves the details of the specified minigame
        /// </summary>
        /// <param name="gameId">game identifier</param>
        /// <param name="minigameId">minigame identifier</param>
        /// <returns>The request minigame</returns>
        [HttpGet("{minigameId}")]
        [ProducesResponseType(typeof(MiniGame), StatusCodes.Status200OK)]
        public IActionResult RetrieveMinigameInfo(string gameId, string minigameId)
        {
            if (!IsUserGameParticipant(gameId) || !IsUserMinigamePlayer(gameId, minigameId))
                return Forbid();

            var minigame = _minigameRepo.Get(minigameId);

            return Ok(MiniGame.FromModel(minigame));
        }

        /// <summary>
        /// Post the answer(s) to the specified minigame
        /// </summary>
        /// <param name="gameId">game identifier</param>
        /// <param name="minigameId">minigame identifier</param>
        /// <param name="answers">answer(s)</param>
        /// <returns></returns>
        [HttpPost("{minigameId}")]
        [ProducesResponseType(typeof(MiniGameResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> AnswerMinigame(string gameId, string minigameId, IEnumerable<string> answers)
        {
            if (!IsUserGameParticipant(gameId) || !IsUserMinigamePlayer(gameId, minigameId))
                return Forbid();

            var minigame = _minigameRepo.Get(minigameId);
            if (minigame.Status !=  Models.MiniGameStatus.Unknown)
                return Forbid();

            minigame.Status = MinigameServiceBase.IsMiniGameAnswerCorrect(minigame, answers)
                ? Models.MiniGameStatus.Win : Models.MiniGameStatus.Lost;

            if (minigame.Status == Models.MiniGameStatus.Win)
            {
                if (minigame.Tile.OwnerId == minigame.PlayerId)
                {
                    //level up
                    minigame.Tile.Difficulty = Math.Min(minigame.Tile.Difficulty + 1, 2);
                }
                else
                {
                    if (string.IsNullOrEmpty(minigame.Tile.OwnerId))
                    {
                        //captured new tile
                        minigame.Tile.ChosenCategoryId = minigame.CategoryId;
                    }
                    minigame.Tile.OwnerId = minigame.PlayerId;
                }
            }

            var game = _gameRepo.Get(gameId);
            game.StepsLeftWithinMove--;
            if(game.StepsLeftWithinMove < 1)
            {
                game.MoveCount++;
                if (game.MoveCount / game.GameUsers.Count >= Models.Game.MaxRounds)
                {
                    var winningPlayerIds = WinningPlayerIds(gameId);
                    foreach (var winnerId in winningPlayerIds)
                    {
                        var user = game.GameUsers.SingleOrDefault(gu => gu.UserId == winnerId);
                        user.IsWinner = true;
                        await _notificationService.SendNotification(user.User, "Congrats", "You won this game on points!");
                    }
                    foreach (var looser in game.GameUsers.Where(gu => !winningPlayerIds.Contains(gu.UserId)))
                    {
                        await _notificationService.SendNotification(looser.User, "Too bad.", "You lost the game!");
                    }
                }
                else
                {
                    //next players move
                    var nextPlayer = game.GameUsers.SingleOrDefault(gu => gu.UserId != game.NextMovePlayerId);
                    game.NextMovePlayerId = nextPlayer.UserId;
                    game.StepsLeftWithinMove = Models.Game.StepsPerPlayer;
                    await _notificationService.SendNotification(nextPlayer.User, "Your turn", "It's your turn!");
                }
            }
            _dataContext.SaveChanges();


            return Ok(MiniGameResult.FromModel(minigame, game, _categoryRepo));
        }

        private bool IsItPlayersTurn(string gameId)
        {
            var game = _gameRepo.Get(gameId);
            return game.NextMovePlayerId == GetCurrentUser().Id;
        }

        private bool IsUserMinigamePlayer(string gameId, string minigameId)
        {
            var user = GetCurrentUser();
            var minigame = _minigameRepo.Get(minigameId);
            return minigame != null && minigame.GameId == gameId && minigame.Player == user;
        }

        private bool HasPlayerAnOpenMinigame(string gameId)
        {
            var user = GetCurrentUser();
            return _minigameRepo.SingleOrDefault(m => m.PlayerId == user.Id && m.GameId == gameId && m.Status == Models.MiniGameStatus.Unknown) != null;
        }

        private bool IsTileInGame(string gameId, string tileId)
        {
            var game = _gameRepo.Get(gameId);
            return game.Tiles.SingleOrDefault(t => t.Id == tileId) != null;
        }

        private bool IsCategoryAllowedForTile(string gameId, string tileId, string categoryId)
        {
            var game = _gameRepo.Get(gameId);
            var tile = game.Tiles.SingleOrDefault(t => t.Id == tileId);
            return (string.IsNullOrWhiteSpace(tile.ChosenCategoryId) || tile.ChosenCategoryId == categoryId) && 
                TileHelper.GetCategoriesForTile(_categoryRepo, tileId).SingleOrDefault(c => c.Id == categoryId) != null;
        }

        private IEnumerable<string> WinningPlayerIds(string gameId)
        {
            var game = _gameRepo.Get(gameId);
            var result = game.GameUsers.ToDictionary(gu => gu.UserId, gu => 0);
            foreach(var user in result)
            {
                result[user.Key] = game.Tiles.Count(t => t.OwnerId == user.Key);
            }
            var rankedPlayers = result.OrderByDescending(r => r.Value);
            return rankedPlayers.Where(p => p.Value >= rankedPlayers.First().Value).Select(p => p.Key).ToList();
        }
    }
}