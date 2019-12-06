﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WikidataGame.Backend.Dto;
using WikidataGame.Backend.Helpers;
using WikidataGame.Backend.Repos;
using WikidataGame.Backend.Services;

namespace WikidataGame.Backend.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GamesController : CustomControllerBase
    {
        private readonly CategoryCacheService _categoryCacheService;
        public GamesController(
            DataContext dataContext,
            UserManager<Models.User> userManager,
            IGameRepository gameRepo,
            CategoryCacheService categoryCacheService,
            INotificationService notificationService) : base(dataContext, userManager, gameRepo, notificationService)
        {
            _categoryCacheService = categoryCacheService;
        }

        /// <summary>
        /// Creates a new game and matches the player with an opponent
        /// </summary>
        /// <returns>Info about the newly created game</returns>
        [HttpPost]
        [ProducesResponseType(typeof(GameInfo), StatusCodes.Status200OK)]
        public async Task<ActionResult<GameInfo>> CreateNewGame()
        {
            var user = await GetCurrentUserAsync();            
            var openGames = await _gameRepo.GetOpenGamesAsync();

            Models.Game game;
            if (openGames.Count() <= 0 || openGames.All(g => g.GameUsers.SingleOrDefault(gu => gu.UserId == user.Id) != null)) 
            {
                //no open games, or only games opened by current player
                game = await _gameRepo.CreateNewGameAsync(user);
            }
            else
            {
                game = openGames.Where(g => g.GameUsers.SingleOrDefault(gu => gu.UserId == user.Id) == null).First();
                game = _gameRepo.JoinGame(game, user);
            }

            await _dataContext.SaveChangesAsync();
            return Ok(GameInfo.FromGame(game));
        }

        /// <summary>
        /// Retrieves all currently running games for the authenticated player
        /// </summary>
        /// <returns>List of game information</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<GameInfo>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<GameInfo>>> GetGames()
        {
            var user = await GetCurrentUserAsync();
            var games = await _gameRepo.RunningGamesForPlayerAsync(user);
            return Ok(games.Select(g => GameInfo.FromGame(g)).ToList());
        }

        /// <summary>
        /// Retrieves information on the specified game
        /// </summary>
        /// <param name="gameId">game identifier</param>
        /// <returns>Info about the specified game</returns>
        [HttpGet("{gameId}")]
        [ProducesResponseType(typeof(Game), StatusCodes.Status200OK)]
        public async Task<ActionResult<Game>> RetrieveGameState(Guid gameId)
        {
            if (!await IsUserGameParticipantAsync(gameId))
                return Forbid();
            var game = await _gameRepo.GetAsync(gameId);

            return Ok(Game.FromModel(game, (await GetCurrentUserAsync()).Id, _categoryCacheService));
        }

        /// <summary>
        /// Stops/deletes the specified game
        /// </summary>
        /// <param name="gameId">game identifier</param>
        /// <returns>204 status code</returns>
        [HttpDelete("{gameId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteGame(Guid gameId)
        {
            if (!await IsUserGameParticipantAsync(gameId))
                return Forbid();

            var game = await _gameRepo.GetAsync(gameId);
            var currentUser = await GetCurrentUserAsync();
            var opponents = game.GameUsers.Select(gu => gu.User).Where(u => u.Id != currentUser.Id).ToList();
            foreach(var opponent in opponents)
            {
                await _notificationService.SendDeleteNotificationAsync(opponent, "Congrats", "You won because your opponent left the game!");
            }
            _dataContext.Set<Models.Tile>().RemoveRange(game.Tiles);
            _gameRepo.Remove(game);
            await _dataContext.SaveChangesAsync();

            return NoContent();
        }
    }
}