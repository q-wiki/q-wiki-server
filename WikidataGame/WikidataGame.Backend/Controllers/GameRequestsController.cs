using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class GameRequestsController : CustomControllerBase
    {
        private readonly IRepository<Models.GameRequest, Guid> _gameRequestRepo;
        public GameRequestsController(
            DataContext dataContext,
            UserManager<Models.User> userManager,
            IGameRepository gameRepo,
            INotificationService notificationService,
            IRepository<Models.GameRequest, Guid> gameRequestRepo)
            : base(dataContext, userManager, gameRepo, notificationService)
        {
            _gameRequestRepo = gameRequestRepo;
        }

        /// <summary>
        /// Retrieves all game requests for the authenticated player
        /// </summary>
        /// <returns>List of game requests</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<GameRequest>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<GameRequest>>> GetGameRequests()
        {
            var user = await GetCurrentUserAsync();
            var gameRequests = await _gameRequestRepo.FindAsync(gr => gr.RecipientId == user.Id || gr.SenderId == user.Id);
            return Ok(gameRequests.Select(gr => GameRequest.FromModel(gr)).ToList());
        }

        /// <summary>
        /// Retrieves all incoming game requests for the authenticated player
        /// </summary>
        /// <returns>List of game requests</returns>
        [HttpGet("Incoming")]
        [ProducesResponseType(typeof(IEnumerable<GameRequest>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<GameRequest>>> GetIncomingGameRequests()
        {
            var user = await GetCurrentUserAsync();
            var gameRequests = await _gameRequestRepo.FindAsync(gr => gr.RecipientId == user.Id);
            return Ok(gameRequests.Select(gr => GameRequest.FromModel(gr)).ToList());
        }

        /// <summary>
        /// Retrieves all outgoing game requests for the authenticated player
        /// </summary>
        /// <returns>List of game requests</returns>
        [HttpGet("Outgoing")]
        [ProducesResponseType(typeof(IEnumerable<GameRequest>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<GameRequest>>> GetOutgoingGameRequests()
        {
            var user = await GetCurrentUserAsync();
            var gameRequests = await _gameRequestRepo.FindAsync(gr => gr.SenderId == user.Id);
            return Ok(gameRequests.Select(gr => GameRequest.FromModel(gr)).ToList());
        }

        /// <summary>
        /// Sends a game request to the specified user
        /// </summary>
        /// <param name="userId">user id</param>
        /// <returns>The created game request</returns>
        [HttpPost]
        [ProducesResponseType(typeof(GameRequest), StatusCodes.Status200OK)]
        public async Task<ActionResult<GameRequest>> RequestMatch(Guid userId)
        {
            var user = await GetCurrentUserAsync();
            var friendUser = await _userManager.FindByIdAsync(userId.ToString());
            if (friendUser == null) //user does not exist
            {
                return NotFound("User not found");
            }
            var similarGameRequests = await _gameRequestRepo.FindAsync(gr => gr.RecipientId == user.Id || gr.SenderId == user.Id);
            var runningGames = await _gameRepo.RunningGamesForPlayerAsync(user);
            if (similarGameRequests.Any() || runningGames.Any(g => g.GameUsers.Any(gu => gu.UserId == friendUser.Id)))
            {
                return Conflict("There already is an open request or running game");
            }

            var request = new Models.GameRequest
            {
                SenderId = user.Id,
                RecipientId = friendUser.Id
            };

            await _gameRequestRepo.AddAsync(request);
            await _dataContext.SaveChangesAsync();

            await _notificationService.SendNotificationAsync(PushType.GameRequest, friendUser, user);

            return Ok(GameRequest.FromModel(request));
        }

        /// <summary>
        /// Deletes a game request for the sender or recipient
        /// </summary>
        /// <param name="gameRequestId">game request identifier</param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteGameRequest(Guid gameRequestId)
        {
            var user = await GetCurrentUserAsync();
            var gameRequest = await _gameRequestRepo.GetAsync(gameRequestId);
            if (gameRequest == null)
            {
                return NotFound("Game request not found");
            }
            if(gameRequest.SenderId != user.Id && gameRequest.RecipientId != user.Id)
            {
                return Forbid();
            }

            _gameRequestRepo.Remove(gameRequest);
            await _dataContext.SaveChangesAsync();
            return NoContent();
        }
    }
}