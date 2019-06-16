using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

        public GamesController(
            DataContext dataContext,
            IUserRepository userRepo,
            IGameRepository gameRepo,
            IRepository<Models.Category, string> categoryRepo) : base(dataContext, userRepo, gameRepo, categoryRepo) {}

        /// <summary>
        /// Creates a new game and matches the player with an opponent
        /// </summary>
        /// <param name="mapWidth">Width of generated map</param>
        /// <param name="mapHeight">Height of generated map</param>
        /// <param name="accessibleTilesCount">How many accessible tiles the generated map should contain.</param>
        /// <returns>Info about the newly created game</returns>
        [HttpPost]
        [ProducesResponseType(typeof(GameInfo), StatusCodes.Status200OK)]
        public IActionResult CreateNewGame(
            int mapWidth = GameConstants.DefaultMapWidth,
            int mapHeight = GameConstants.DefaultMapHeight,
            int accessibleTilesCount = GameConstants.DefaultAccessibleTilesCount)
        {
            var user = GetCurrentUser();            
            var game = _gameRepo.RunningGameForPlayer(user);
            if (game == default(Models.Game))
            {
                game = _gameRepo.GetOpenGame();
                if (game == default(Models.Game))
                {
                    game = _gameRepo.CreateNewGame(user, mapWidth, mapHeight, accessibleTilesCount);
                }
                else
                {
                    _gameRepo.JoinGame(game, user);
                }

                _dataContext.SaveChanges();
            }

            return Ok(GameInfo.FromGame(game));
        }

        /// <summary>
        /// Retrieves information on the specified game
        /// </summary>
        /// <param name="gameId">game identifier</param>
        /// <returns>Info about the specified game</returns>
        [HttpGet("{gameId}")]
        [ProducesResponseType(typeof(Game), StatusCodes.Status200OK)]
        public IActionResult RetrieveGameState(string gameId)
        {
            if (!IsUserGameParticipant(gameId))
                return Forbid();
            var game = _gameRepo.Get(gameId);

            return Ok(Game.FromModel(game, GetCurrentUser().Id, _categoryRepo));
        }

        /// <summary>
        /// Stops/deletes the specified game
        /// </summary>
        /// <param name="gameId">game identifier</param>
        /// <returns>204 status code</returns>
        [HttpDelete("{gameId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult DeleteGame(string gameId)
        {
            if (!IsUserGameParticipant(gameId))
                return Forbid();

            //TODO: notify opponent
            var game = _gameRepo.Get(gameId);
            _gameRepo.Remove(game);
            _dataContext.SaveChanges();

            return NoContent();
        }
    }
}