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
        private readonly IQuestionRepository _questionRepo;
        private readonly CategoryCacheService _categoryCacheService;

        public MinigamesController(
            DataContext dataContext,
            IUserRepository userRepo,
            IGameRepository gameRepo,
            IMinigameRepository minigameRepo,
            IRepository<Models.Category, string> categoryRepo,
            IQuestionRepository questionRepo,
            INotificationService notificationService,
            CategoryCacheService categoryCacheService) : base(dataContext, userRepo, gameRepo, categoryRepo, notificationService)
        {
            _minigameRepo = minigameRepo;
            _questionRepo = questionRepo;
            _categoryCacheService = categoryCacheService;
        }

        /// <summary>
        /// Initializes a new minigame
        /// </summary>
        /// <param name="gameId">game identifier</param>
        /// <param name="minigameParams">minigame information containing category and tile identifier</param>
        /// <returns>The created minigame</returns>
        [HttpPost]
        [ProducesResponseType(typeof(MiniGame), StatusCodes.Status200OK)]
        public async Task<ActionResult<MiniGame>> InitalizeMinigame(string gameId, MiniGameInit minigameParams)
        {
            if (!await IsUserGameParticipantAsync(gameId) || minigameParams == null ||
                !await IsTileInGameAsync(gameId, minigameParams.TileId) ||
                !await IsCategoryAllowedForTileAsync(gameId, minigameParams.TileId, minigameParams.CategoryId) ||
                !await IsItPlayersTurnAsync(gameId) ||
                await HasPlayerAnOpenMinigameAsync(gameId))
                    return Forbid();

            var minigameServices = ControllerContext.HttpContext.RequestServices.GetServices<IMinigameService>();
            var question = await _questionRepo.GetRandomQuestionForCategoryAsync(minigameParams.CategoryId);
            var service = minigameServices.SingleOrDefault(s => s.MiniGameType == question.MiniGameType);

            var minigame = await service.GenerateMiniGameAsync(gameId, (await GetCurrentUserAsync()).Id, question, minigameParams.TileId);

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
        public async Task<ActionResult<MiniGame>> RetrieveMinigameInfo(string gameId, string minigameId)
        {
            if (!await IsUserGameParticipantAsync(gameId) || !await IsUserMinigamePlayerAsync(gameId, minigameId))
                return Forbid();

            var minigame = await _minigameRepo.GetAsync(minigameId);

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
        public async Task<ActionResult<MiniGameResult>> AnswerMinigame(string gameId, string minigameId, IEnumerable<string> answers)
        {
            if (!await IsUserGameParticipantAsync(gameId) || !await IsUserMinigamePlayerAsync(gameId, minigameId))
                return Forbid();

            var minigame = await _minigameRepo.GetAsync(minigameId);
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
                    minigame.Tile.ChosenCategoryId = minigame.CategoryId;
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

            var game = await _gameRepo.GetAsync(gameId);
            game.StepsLeftWithinMove--;
            if(await AllTilesConqueredAsync(gameId))
            {
                await SetGameWonAsync(game);
            }

            if(game.StepsLeftWithinMove < 1)
            {
                game.MoveCount++;
                if (game.MoveCount / game.GameUsers.Count >= Models.Game.MaxRounds)
                {
                    await SetGameWonAsync(game);
                }
                else
                {
                    //next players move
                    var nextPlayer = game.GameUsers.SingleOrDefault(gu => gu.UserId != game.NextMovePlayerId);
                    game.NextMovePlayerId = nextPlayer.UserId;
                    game.MoveStartedAt = DateTime.UtcNow;
                    game.StepsLeftWithinMove = Models.Game.StepsPerPlayer;
                    await _notificationService.SendNotificationWithRefreshAsync(
                        nextPlayer.User,
                        "It's your turn!",
                        "You have 12 hours left to play your round.");
                }
            }
            await _dataContext.SaveChangesAsync();

            return Ok(MiniGameResult.FromModel(minigame, game, _categoryCacheService));
        }

        private async Task<bool> IsItPlayersTurnAsync(string gameId)
        {
            var game = await _gameRepo.GetAsync(gameId);
            return game.NextMovePlayerId == (await GetCurrentUserAsync()).Id;
        }

        private async Task<bool> IsUserMinigamePlayerAsync(string gameId, string minigameId)
        {
            var user = await GetCurrentUserAsync();
            var minigame = await _minigameRepo.GetAsync(minigameId);
            return minigame != null && minigame.GameId == gameId && minigame.Player == user;
        }

        private async Task<bool> HasPlayerAnOpenMinigameAsync(string gameId)
        {
            var user = await GetCurrentUserAsync();
            return await _minigameRepo.SingleOrDefaultAsync(m => m.PlayerId == user.Id && m.GameId == gameId && m.Status == Models.MiniGameStatus.Unknown) != null;
        }

        private async Task<bool> IsTileInGameAsync(string gameId, string tileId)
        {
            var game = await _gameRepo.GetAsync(gameId);
            return game.Tiles.SingleOrDefault(t => t.Id == tileId) != null;
        }

        private async Task<bool> IsCategoryAllowedForTileAsync(string gameId, string tileId, string categoryId)
        {
            var game = await _gameRepo.GetAsync(gameId);
            var tile = game.Tiles.SingleOrDefault(t => t.Id == tileId);
            return (string.IsNullOrWhiteSpace(tile.ChosenCategoryId) || tile.ChosenCategoryId == categoryId) && 
                (await TileHelper.GetCategoriesForTileAsync(_categoryCacheService, tileId)).SingleOrDefault(c => c.Id == categoryId) != null;
        }

        private async Task<IEnumerable<string>> WinningPlayerIdsAsync(string gameId)
        {
            var game = await _gameRepo.GetAsync(gameId);
            var result = game.GameUsers.ToDictionary(gu => gu.UserId, gu => 0);
            var tiles = game.Tiles.ToList();
            foreach (var tile in tiles)
            {
                if (!string.IsNullOrEmpty(tile.OwnerId))
                {
                    result[tile.OwnerId] += tile.Difficulty + 1;
                }
            }
            var rankedPlayers = result.OrderByDescending(r => r.Value);
            return rankedPlayers.Where(p => p.Value >= rankedPlayers.First().Value).Select(p => p.Key).ToList();
        }

        private async Task<bool> AllTilesConqueredAsync(string gameId)
        {
            var game = await _gameRepo.GetAsync(gameId);
            var currentUserId = (await GetCurrentUserAsync()).Id;
            var opponentId = game.GameUsers.SingleOrDefault(gu => gu.UserId != currentUserId).UserId;
            return game.Tiles.Count(t => t.OwnerId == opponentId) < 1;
        }

        private async Task SetGameWonAsync(Models.Game game)
        {
            var winningPlayerIds = await WinningPlayerIdsAsync(game.Id);
            foreach (var winnerId in winningPlayerIds)
            {
                var user = game.GameUsers.SingleOrDefault(gu => gu.UserId == winnerId);
                user.IsWinner = true;
                await _notificationService.SendNotificationAsync(user.User, "Congrats", "You won this game on points!");
            }
            foreach (var looser in game.GameUsers.Where(gu => !winningPlayerIds.Contains(gu.UserId)))
            {
                await _notificationService.SendNotificationAsync(looser.User, "Too bad.", "You lost the game! Start a new game for another chance.");
            }
        }
    }
}