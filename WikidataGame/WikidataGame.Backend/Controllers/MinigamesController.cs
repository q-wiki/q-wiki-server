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

        public MinigamesController(
            DataContext dataContext,
            IUserRepository userRepo,
            IGameRepository gameRepo,
            IMinigameRepository minigameRepo) : base(dataContext, userRepo, gameRepo)
        {
            _minigameRepo = minigameRepo;
        }

        /// <summary>
        /// Initializes a new minigame
        /// </summary>
        /// <param name="gameId">game identifier</param>
        /// <param name="minigameParams">minigame information containing category and tile identifier</param>
        /// <returns>The created minigame</returns>
        [HttpPost]
        [ProducesResponseType(typeof(MiniGame), StatusCodes.Status200OK)]
        public IActionResult InitalizeMinigame(string gameId, MiniGameInit minigameParams)
        {
            if (!IsUserGameParticipant(gameId))
                return Forbid();
            //TODO: check if category allowed

            var minigameServices = ControllerContext.HttpContext.RequestServices.GetServices<IMinigameService>();
            var random = new Random();
            var randomService = minigameServices.ElementAt(random.Next(0, minigameServices.Count() - 1));

            //TODO: Implement sorting and image minigames!
            // TODO: remove hard-coded category id
            var minigame = randomService.GenerateMiniGame(gameId, GetCurrentUser().Id, "cf3111af-8b18-4c6f-8ee6-115157d54b79");

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
        public IActionResult RetrieveMinigameInfo(string gameId, string minigameId)
        {
            if (!IsUserGameParticipant(gameId) || !IsUserMinigamePlayer(gameId, minigameId))
                return Forbid();

            //TODO: check if game exists
            var minigame = _minigameRepo.Get(minigameId);

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
        public IActionResult AnswerMinigame(string gameId, string minigameId, IEnumerable<string> answers)
        {
            if (!IsUserGameParticipant(gameId) || !IsUserMinigamePlayer(gameId, minigameId))
                return Forbid();

            var minigame = _minigameRepo.Get(minigameId);
            minigame.IsWin = MinigameServiceBase.IsMiniGameAnswerCorrect(minigame, answers);
            // TODO: adapt tiles to result

            _dataContext.SaveChanges();

            var game = _gameRepo.Get(gameId);

            return Ok(MiniGameResult.FromModel(minigame, game));
        }

        private bool IsUserMinigamePlayer(string gameId, string minigameId)
        {
            var user = GetCurrentUser();
            var minigame = _minigameRepo.Get(minigameId);
            return minigame != null && minigame.GameId == gameId && minigame.Player == user;
        }
    }
}