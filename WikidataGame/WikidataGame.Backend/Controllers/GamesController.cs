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
        private readonly AppSettings _appSettings;
        private readonly IGameRepository _gameRepo;

        public GamesController(
            DataContext dataContext,
            IUserRepository userRepo,
            IGameRepository gameRepo,
            IOptions<AppSettings> appSettings) : base(dataContext, userRepo, gameRepo)
        {
            _gameRepo = gameRepo;
            _appSettings = appSettings.Value;
        }

        /// <summary>
        /// Creates a new game and matches the player with an opponent
        /// </summary>
        /// <param name="deviceId">device identifier</param>
        /// <param name="pushUrl">push notification channel url</param>
        /// <returns>Info about the newly created game</returns>
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(GameInfo), StatusCodes.Status200OK)]
        public IActionResult CreateNewGame(
            [FromHeader(Name = "X-Device-ID")] string deviceId,
            [FromHeader(Name = "X-Push-URL")] string pushUrl)
        {
            if(string.IsNullOrWhiteSpace(deviceId))
                return BadRequest(new { message = "DeviceId needs to be supplied" });

            var user = _userRepo.CreateOrUpdateUser(deviceId, pushUrl);
            _dataContext.SaveChanges();

            Response.Headers.Add("WWW-Authenticate", $"Bearer {JwtTokenHelper.CreateJwtToken(deviceId, _appSettings)}");

            var game = _gameRepo.RunningGameForPlayer(user);
            if (game == default(Models.Game))
            {
                game = _gameRepo.GetOpenGame();
                if (game == default(Models.Game))
                {
                    game = _gameRepo.CreateNewGame(user);
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
                Forbid();
            var game = _gameRepo.Get(gameId);

            return Ok(Game.FromModel(game, GetCurrentUser().DeviceId));
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
                Forbid();

            //TODO: notify opponent
            var game = _gameRepo.Get(gameId);
            _gameRepo.Remove(game);

            return NoContent();
        }
    }
}