using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WikidataGame.Backend.Dto;
using WikidataGame.Backend.Helpers;
using WikidataGame.Backend.Repos;

namespace WikidataGame.Backend.Controllers
{
    [Route("api/games/{gameId}/minigames")]
    [Authorize]
    [ApiController]
    public class MinigamesController : CustomControllerBase
    {
        public MinigamesController(
            DataContext dataContext,
            IUserRepository userRepo) : base(dataContext, userRepo)
        {

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
            //TODO: check if game exists
            //TODO: check if category allowed

            return Ok(new MiniGame {
                Id = Guid.NewGuid().ToString(),
                Type = MiniGameType.BlurryImage,
                AnswerOptions = new List<string> { "Elephant", "Zebra", "Tiger", "Dog" },
                TaskDescription = "Guess as fast as you can what is shown on the image, which will get sharper from time to time"
            });
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
            //TODO: check if game exists
            return Ok(new MiniGame
            {
                Id = Guid.NewGuid().ToString(),
                Type = MiniGameType.BlurryImage,
                AnswerOptions = new List<string> { "Elephant", "Zebra", "Tiger", "Dog" },
                TaskDescription = "Guess as fast as you can what is shown on the image. The image is blurred and will get sharper from time to time."
            });
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
            //TODO: check if game exists
            //TODO: check if allowed to answer

            return Ok(new MiniGameResult
            {
                IsWin = answers != null && answers.Count() > 0 && answers.First() == "Elephant",
                CorrectAnswer = new List<string> { "Elephant" },
                Tiles = new List<Tile>()
            });
        }
    }
}