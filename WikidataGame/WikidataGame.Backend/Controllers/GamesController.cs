using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
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
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GamesController : CustomControllerBase
    {
        private readonly CategoryCacheService _categoryCacheService;
        private readonly IRepository<Models.GameRequest, Guid> _gameRequestRepo;

        public GamesController(
            DataContext dataContext,
            UserManager<Models.User> userManager,
            IGameRepository gameRepo,
            CategoryCacheService categoryCacheService,
            INotificationService notificationService,
            IMapper mapper,
            IRepository<Models.GameRequest, Guid> gameRequestRepo) : base(dataContext, userManager, gameRepo, notificationService, mapper)
        {
            _categoryCacheService = categoryCacheService;
            _gameRequestRepo = gameRequestRepo;
        }

        /// <summary>
        /// Creates a new game by accepting a game request
        /// </summary>
        /// <param name="gameRequestId">game request identifier</param>
        /// <returns>Info about the newly created game</returns>
        [HttpPost("AcceptRequest")]
        [ProducesResponseType(typeof(GameInfo), StatusCodes.Status201Created)]
        public async Task<ActionResult<GameInfo>> CreateNewGameByRequest(Guid gameRequestId)
        {
            var user = await GetCurrentUserAsync();
            // create game from request
            var gameRequest = await _gameRequestRepo.GetAsync(gameRequestId);
            if (gameRequest == null)
            {
                return NotFound("Game request not found");
            }
            if (gameRequest.RecipientId != user.Id)
            {
                return Forbid();
            }
            _gameRequestRepo.Remove(gameRequest);
            var game = await _gameRepo.CreateNewGameAsync(gameRequest.Sender);
            _gameRepo.JoinGame(game, user);
            

            await _dataContext.SaveChangesAsync();
            return Created(string.Empty, _mapper.Map<GameInfo>(game));
        }

        /// <summary>
        /// Creates a new game and matches the player with an opponent
        /// </summary>
        /// <returns>Info about the newly created game</returns>
        [HttpPost]
        [ProducesResponseType(typeof(GameInfo), StatusCodes.Status201Created)]
        public async Task<ActionResult<GameInfo>> CreateNewGame()
        {
            var user = await GetCurrentUserAsync();
            
            //find matching player
            var availableGames = await _gameRepo.GetGamesForUserToJoinAsync(user);
            Models.Game game;
            if (availableGames.Count() <= 0)
            {
                //no open games, or only games opened by current player, or only open games with a player the current user is already playing with
                game = await _gameRepo.CreateNewGameAsync(user);
            }
            else
            {
                game = _gameRepo.JoinGame(availableGames.First(), user);
            }

            await _dataContext.SaveChangesAsync();
            return Created(string.Empty, _mapper.Map<GameInfo>(game));
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
            return Ok(games.Select(g => _mapper.Map<GameInfo>(g)).ToList());
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

            var userId = (await GetCurrentUserAsync()).Id;
            return Ok(_mapper.Map<Game>(game, opt => opt.Items[nameof(Models.GameUser.UserId)] = userId));
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
                await _notificationService.SendNotificationAsync(PushType.Delete, opponent, currentUser, game.Id);
            }
            _dataContext.Set<Models.Tile>().RemoveRange(game.Tiles);
            _gameRepo.Remove(game);
            await _dataContext.SaveChangesAsync();

            return NoContent();
        }
    }
}