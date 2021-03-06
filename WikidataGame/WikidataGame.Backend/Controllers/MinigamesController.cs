﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
    public class MinigamesController : ControllerBase
    {
        /// <summary>
        /// Initializes a new minigame
        /// </summary>
        /// <param name="gameId">game identifier</param>
        /// <param name="minigameParams">minigame information containing category and tile identifier</param>
        /// <returns>The created minigame</returns>
        [HttpPost]
        [ProducesResponseType(typeof(MiniGame), StatusCodes.Status201Created)]
        public async Task<ActionResult<MiniGame>> InitalizeMinigame(
            Guid gameId,
            MiniGameInit minigameParams,
#pragma warning disable CS1573 // no xml comments for service injection
            [FromServices] CategoryCacheService ccs,
            [FromServices] IMinigameRepository minigameRepo,
            [FromServices] IQuestionRepository questionRepo,
            [FromServices] IGameRepository gameRepo,
            [FromServices] UserManager<Models.User> userManager,
            [FromServices] IMapper mapper)
#pragma warning restore CS1573
        {
            var user = await userManager.GetUserAsync(User);
            if (!await gameRepo.IsUserGameParticipantAsync(user, gameId) || minigameParams == null ||
                !await gameRepo.IsTileInGameAsync(gameId, minigameParams.TileId) ||
                !await gameRepo.IsCategoryAllowedForTileAsync(ccs, gameId, minigameParams.TileId, minigameParams.CategoryId) ||
                !await gameRepo.IsItPlayersTurnAsync(user, gameId) ||
                await minigameRepo.HasPlayerAnOpenMinigameAsync(user, gameId))
                    return Forbid();

            var minigameServices = ControllerContext.HttpContext.RequestServices.GetServices<IMinigameService>();
            var question = questionRepo.GetRandomQuestionForCategory(minigameParams.CategoryId);
            var service = minigameServices.SingleOrDefault(s => s.MiniGameType == question.MiniGameType);

            try
            {
                var minigame = await service.GenerateMiniGameAsync(gameId, user.Id, question, minigameParams.TileId);
                return Created(string.Empty, mapper.Map<MiniGame>(minigame));
            }
            catch(Exception)
            {
                //second try
                try
                {
                    var minigame = await service.GenerateMiniGameAsync(gameId, user.Id, question, minigameParams.TileId);
                    return Created(string.Empty, mapper.Map<MiniGame>(minigame));
                }
                catch (Exception)
                {
                    return StatusCode(503);
                }
            }

        }

        /// <summary>
        /// Retrieves the details of the specified minigame
        /// </summary>
        /// <param name="gameId">game identifier</param>
        /// <param name="minigameId">minigame identifier</param>
        /// <returns>The request minigame</returns>
        [HttpGet("{minigameId}")]
        [ProducesResponseType(typeof(MiniGame), StatusCodes.Status200OK)]
        public async Task<ActionResult<MiniGame>> RetrieveMinigameInfo(
            Guid gameId,
            Guid minigameId,
#pragma warning disable CS1573 // no xml comments for service injection
            [FromServices] IGameRepository gameRepo,
            [FromServices] IMinigameRepository minigameRepo,
            [FromServices] UserManager<Models.User> userManager,
            [FromServices] IMapper mapper)
#pragma warning restore CS1573
        {
            var user = await userManager.GetUserAsync(User);
            if (!await gameRepo.IsUserGameParticipantAsync(user, gameId) ||
                !await minigameRepo.IsUserMinigamePlayerAsync(user, gameId, minigameId))
                return Forbid();

            var minigame = await minigameRepo.GetAsync(minigameId);

            return Ok(mapper.Map<MiniGame>(minigame));
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
        public async Task<ActionResult<MiniGameResult>> AnswerMinigame(
            Guid gameId,
            Guid minigameId,
            IEnumerable<string> answers,
#pragma warning disable CS1573 // no xml comments for service injection
            [FromServices] IGameRepository gameRepo,
            [FromServices] IMinigameRepository minigameRepo,
            [FromServices] UserManager<Models.User> userManager,
            [FromServices] IMapper mapper,
            [FromServices] INotificationService notificationService,
            [FromServices] CategoryCacheService ccs,
            [FromServices] DataContext dataContext)
#pragma warning restore CS1573
        {
            var user = await userManager.GetUserAsync(User);
            if (!await gameRepo.IsUserGameParticipantAsync(user, gameId) ||
                !await minigameRepo.IsUserMinigamePlayerAsync(user, gameId, minigameId))
                return Forbid();

            var minigame = await minigameRepo.GetAsync(minigameId);
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
                    if (minigame.Tile.OwnerId == default)
                    {
                        //captured new tile
                        minigame.Tile.ChosenCategoryId = minigame.CategoryId;
                    }
                    minigame.Tile.OwnerId = minigame.PlayerId;
                }
            }

            await FinalizeMove(gameRepo, notificationService, user, gameId);
            await dataContext.SaveChangesAsync();
            var game = await gameRepo.GetAsync(gameId);
            while(game.NextMovePlayerId == DatabaseSeeds.BotGuid) //Bot turn
            {
                BotTurn(game, ccs);
                await FinalizeMove(gameRepo, notificationService, await userManager.FindByIdAsync(DatabaseSeeds.BotGuid.ToString()), gameId);
                await dataContext.SaveChangesAsync();
            }

            return Ok(mapper.Map<MiniGameResult>(minigame));
        }

        private void BotTurn(Models.Game game, CategoryCacheService ccs)
        {
            var random = new Random();
            var map = game.Tiles.OrderBy(t => t.MapIndex);
            var botTiles = map.Where(t => t.OwnerId == DatabaseSeeds.BotGuid).ToList();
            Models.Tile tile;
            if (random.NextDouble() > 0.2d)
            {
                //Find tile with shortest route to opponent
                var opponentTiles = map.Where(t => t.OwnerId != DatabaseSeeds.BotGuid && t.OwnerId != null).ToList();
                tile = TileHelper.FindTileForShortestPath(botTiles, opponentTiles, game);
            }
            else
            {
                //level up existing tile
                tile = botTiles.OrderBy(_ => Guid.NewGuid()).First();
                if (tile.ChosenCategoryId == null) //start tile
                {
                    var availableCategories = TileHelper.GetCategoriesForTile(ccs, tile.Id);
                    tile.ChosenCategoryId = availableCategories.OrderBy(_ => Guid.NewGuid()).First().Id;
                }
            }
            var certainty = random.NextDouble() * 0.9d - (tile.Difficulty / 10d);
            if (certainty > 0.4d) //win
            {
                if (tile.OwnerId == default)
                {
                    var availableCategories = TileHelper.GetCategoriesForTile(ccs, tile.Id);
                    tile.ChosenCategoryId = availableCategories.OrderBy(_ => Guid.NewGuid()).First().Id;
                    tile.OwnerId = DatabaseSeeds.BotGuid;
                }
                else if (tile.OwnerId != DatabaseSeeds.BotGuid)
                {
                    tile.OwnerId = DatabaseSeeds.BotGuid;
                }
                else
                {
                    tile.Difficulty = Math.Min(tile.Difficulty + 1, 2);
                }
            }
        }

        private async Task FinalizeMove(
            IGameRepository gameRepo,
            INotificationService notificationService,
            Models.User user,
            Guid gameId)
        {
            var game = await gameRepo.GetAsync(gameId);
            game.StepsLeftWithinMove--;
            if (await gameRepo.AllTilesConqueredAsync(user, gameId))
            {
                await gameRepo.SetGameWonAsync(game, notificationService);
            }
            else if (game.StepsLeftWithinMove < 1)
            {
                game.MoveCount++;
                if (game.MoveCount / game.GameUsers.Count >= Models.Game.MaxRounds)
                {
                    await gameRepo.SetGameWonAsync(game, notificationService);
                }
                else
                {
                    //next players move
                    var nextPlayer = game.GameUsers.SingleOrDefault(gu => gu.UserId != game.NextMovePlayerId);
                    game.NextMovePlayerId = nextPlayer.UserId;
                    game.MoveStartedAt = DateTime.UtcNow;
                    game.StepsLeftWithinMove = Models.Game.StepsPerPlayer;
                    await notificationService.SendNotificationAsync(PushType.YourTurn, nextPlayer.User, user, game.Id);
                }
            }
        }
    }
}