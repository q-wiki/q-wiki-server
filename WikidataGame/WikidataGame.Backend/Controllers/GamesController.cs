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

namespace WikidataGame.Backend.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GamesController : CustomControllerBase
    {
        private readonly AppSettings _appSettings;

        public GamesController(
            DataContext dataContext,
            IRepository<Models.User, string> userRepo,
            IOptions<AppSettings> appSettings) : base(dataContext, userRepo)
        {
            
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

            //create or update user
            var user = _userRepo.Get(deviceId);
            if (user == null)
            {
                _userRepo.Add(new Models.User
                {
                    DeviceId = deviceId,
                    PushChannelUrl = pushUrl
                });
            }
            else
            {
                user.PushChannelUrl = pushUrl;
                _userRepo.Update(user);
            }
            _dataContext.SaveChanges();

            Response.Headers.Add("WWW-Authenticate", $"Bearer {JwtTokenHelper.CreateJwtToken(deviceId, _appSettings)}");
            return Ok (new GameInfo
            {
                GameId = Guid.NewGuid().ToString(),
                IsAwaitingOpponentToJoin = true,
                Message = "Hello World!"
            });
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
            //check if game exists for user
            var user = GetCurrentUser();

            return Ok(new Game
            {
                Id = gameId
            });
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
            //check if game exists

            return NoContent();
        }
    }
}