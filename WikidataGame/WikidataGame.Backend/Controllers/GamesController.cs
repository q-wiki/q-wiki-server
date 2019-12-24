using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WikidataGame.Backend.Dto;
using WikidataGame.Backend.Helpers;
using WikidataGame.Backend.Repos;
using WikidataGame.Backend.Services;

namespace WikidataGame.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GamesController : ControllerBase
    {
        /// <summary>
        /// Creates a new game by accepting a game request
        /// </summary>
        /// <param name="gameRequestId">game request identifier</param>
        /// <returns>Info about the newly created game</returns>
        [HttpPost("AcceptRequest")]
        [ProducesResponseType(typeof(GameInfo), StatusCodes.Status201Created)]
        public async Task<ActionResult<GameInfo>> CreateNewGameByRequest(
            Guid gameRequestId,
#pragma warning disable CS1573 // no xml comments for service injection
            [FromServices] UserManager<Models.User> userManager,
            [FromServices] IRepository<Models.GameRequest, Guid> gameRequestRepo,
            [FromServices] IGameRepository gameRepo,
            [FromServices] DataContext dataContext,
            [FromServices] IMapper mapper)
#pragma warning restore CS1573
        {
            var user = await userManager.GetUserAsync(User);
            // create game from request
            var gameRequest = await gameRequestRepo.GetAsync(gameRequestId);
            if (gameRequest == null)
            {
                return NotFound("Game request not found");
            }
            if (gameRequest.RecipientId != user.Id)
            {
                return Forbid();
            }
            gameRequestRepo.Remove(gameRequest);
            var game = await gameRepo.CreateNewGameAsync(gameRequest.Sender);
            gameRepo.JoinGame(game, user);
            

            await dataContext.SaveChangesAsync();
            return Created(string.Empty, mapper.Map<GameInfo>(game));
        }

        /// <summary>
        /// Creates a new game and matches the player with an opponent
        /// </summary>
        /// <returns>Info about the newly created game</returns>
        [HttpPost]
        [ProducesResponseType(typeof(GameInfo), StatusCodes.Status201Created)]
        public async Task<ActionResult<GameInfo>> CreateNewGame(
            [FromServices] UserManager<Models.User> userManager,
            [FromServices] IGameRepository gameRepo,
            [FromServices] DataContext dataContext,
            [FromServices] IMapper mapper)
        {
            var user = await userManager.GetUserAsync(User);
            
            //find matching player
            var availableGames = await gameRepo.GetGamesForUserToJoinAsync(user);
            Models.Game game;
            if (availableGames.Count() <= 0)
            {
                //no open games, or only games opened by current player, or only open games with a player the current user is already playing with
                game = await gameRepo.CreateNewGameAsync(user);
            }
            else
            {
                game = gameRepo.JoinGame(availableGames.First(), user);
            }

            await dataContext.SaveChangesAsync();
            return Created(string.Empty, mapper.Map<GameInfo>(game));
        }

        /// <summary>
        /// Retrieves all currently running games for the authenticated player
        /// </summary>
        /// <returns>List of game information</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<GameInfo>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<GameInfo>>> GetGames(
            [FromServices] UserManager<Models.User> userManager,
            [FromServices] IGameRepository gameRepo,
            [FromServices] IMapper mapper)
        {
            var user = await userManager.GetUserAsync(User);
            var games = await gameRepo.RunningGamesForPlayerAsync(user);
            return Ok(games.Select(g => mapper.Map<GameInfo>(g)).ToList());
        }

        /// <summary>
        /// Retrieves information on the specified game
        /// </summary>
        /// <param name="gameId">game identifier</param>
        /// <returns>Info about the specified game</returns>
        [HttpGet("{gameId}")]
        [ProducesResponseType(typeof(Game), StatusCodes.Status200OK)]
        public async Task<ActionResult<Game>> RetrieveGameState(
            Guid gameId,
#pragma warning disable CS1573 // no xml comments for service injection
            [FromServices] UserManager<Models.User> userManager,
            [FromServices] IGameRepository gameRepo,
            [FromServices] IMapper mapper)
#pragma warning restore CS1573
        {
            var user = await userManager.GetUserAsync(User);
            if (!await gameRepo.IsUserGameParticipantAsync(user, gameId))
                return Forbid();
            var game = await gameRepo.GetAsync(gameId);

            return Ok(mapper.Map<Game>(game, opt => opt.Items[nameof(Models.GameUser.UserId)] = user.Id));
        }

        /// <summary>
        /// Stops/deletes the specified game
        /// </summary>
        /// <param name="gameId">game identifier</param>
        /// <returns>204 status code</returns>
        [HttpDelete("{gameId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteGame(
            Guid gameId,
#pragma warning disable CS1573 // no xml comments for service injection
            [FromServices] UserManager<Models.User> userManager,
            [FromServices] IGameRepository gameRepo,
            [FromServices] DataContext dataContext,
            [FromServices] INotificationService notificationService)
#pragma warning restore CS1573
        {
            var user = await userManager.GetUserAsync(User);
            if (!await gameRepo.IsUserGameParticipantAsync(user, gameId))
                return Forbid();

            var game = await gameRepo.GetAsync(gameId);
            var opponents = game.GameUsers.Select(gu => gu.User).Where(u => u.Id != user.Id).ToList();
            foreach(var opponent in opponents)
            {
                await notificationService.SendNotificationAsync(PushType.Delete, opponent, user, game.Id);
            }
            dataContext.Set<Models.Tile>().RemoveRange(game.Tiles);
            gameRepo.Remove(game);
            await dataContext.SaveChangesAsync();

            return NoContent();
        }
    }
}