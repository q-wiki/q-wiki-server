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
    public class GameRequestsController : ControllerBase
    {
        /// <summary>
        /// Retrieves all game requests for the authenticated player
        /// </summary>
        /// <returns>Two list of game requests (incoming/outgoing)</returns>
        [HttpGet]
        [ProducesResponseType(typeof(GameRequestList), StatusCodes.Status200OK)]
        public async Task<ActionResult<GameRequestList>> GetGameRequests(
            [FromServices] UserManager<Models.User> userManager,
            [FromServices] IRepository<Models.GameRequest, Guid> gameRequestRepo,
            [FromServices] IMapper mapper)
        {
            var user = await userManager.GetUserAsync(User);
            var incomingRequests = await gameRequestRepo.FindAsync(gr => gr.RecipientId == user.Id);
            var outgoingRequests = await gameRequestRepo.FindAsync(gr => gr.SenderId == user.Id);
            var gameRequestList = new GameRequestList {
                Incoming = incomingRequests.Select(gr => mapper.Map<GameRequest>(gr)).ToList(),
                Outgoing = outgoingRequests.Select(gr => mapper.Map<GameRequest>(gr)).ToList()
            };
            return Ok(gameRequestList);
        }

        /// <summary>
        /// Sends a game request to the specified user
        /// </summary>
        /// <param name="userId">user id</param>
        /// <returns>The created game request</returns>
        [HttpPost]
        [ProducesResponseType(typeof(GameRequest), StatusCodes.Status201Created)]
        public async Task<ActionResult<GameRequest>> RequestMatch(
            Guid userId,
#pragma warning disable CS1573 // no xml comments for service injection
            [FromServices] UserManager<Models.User> userManager,
            [FromServices] IRepository<Models.GameRequest, Guid> gameRequestRepo,
            [FromServices] IGameRepository gameRepo,
            [FromServices] DataContext dataContext,
            [FromServices] INotificationService notificationService,
            [FromServices] IMapper mapper)
#pragma warning restore CS1573
        {
            var user = await userManager.GetUserAsync(User);
            var friendUser = await userManager.FindByIdAsync(userId.ToString());
            if (friendUser == null) //user does not exist
            {
                return NotFound("User not found");
            }
            var similarGameRequests = await gameRequestRepo.FindAsync(gr => gr.RecipientId == user.Id && gr.SenderId == user.Id);
            var runningGames = await gameRepo.RunningGamesForPlayerAsync(user);
            if (similarGameRequests.Any() || runningGames.Any(g => g.GameUsers.Any(gu => gu.UserId == friendUser.Id)))
            {
                return Conflict("There already is an open request or running game");
            }

            var request = new Models.GameRequest
            {
                SenderId = user.Id,
                RecipientId = friendUser.Id
            };

            await gameRequestRepo.AddAsync(request);
            await dataContext.SaveChangesAsync();

            await notificationService.SendNotificationAsync(PushType.GameRequest, friendUser, user);

            return Created(string.Empty, mapper.Map<GameRequest>(request));
        }

        /// <summary>
        /// Deletes a game request for the sender or recipient
        /// </summary>
        /// <param name="gameRequestId">game request identifier</param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteGameRequest(
            Guid gameRequestId,
#pragma warning disable CS1573 // no xml comments for service injection
            [FromServices] UserManager<Models.User> userManager,
            [FromServices] IRepository<Models.GameRequest, Guid> gameRequestRepo,
            [FromServices] DataContext dataContext)
#pragma warning restore CS1573
        {
            var user = await userManager.GetUserAsync(User);
            var gameRequest = await gameRequestRepo.GetAsync(gameRequestId);
            if (gameRequest == null)
            {
                return NotFound("Game request not found");
            }
            if(gameRequest.SenderId != user.Id && gameRequest.RecipientId != user.Id)
            {
                return Forbid();
            }

            gameRequestRepo.Remove(gameRequest);
            await dataContext.SaveChangesAsync();
            return NoContent();
        }
    }
}