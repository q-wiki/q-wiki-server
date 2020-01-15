using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WikidataGame.Backend.Dto;
using WikidataGame.Backend.Helpers;
using WikidataGame.Backend.Repos;

namespace WikidataGame.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAny")]
    public class PlatformController : ControllerBase
    {
        /// <summary>
        /// Retrieves statistics containing the number of categories, games played and questions added
        /// </summary>
        /// <returns>Current statistics</returns>
        [HttpGet("Stats")]
        [ProducesResponseType(typeof(PlatformStats), StatusCodes.Status200OK)]
        public async Task<ActionResult<PlatformStats>> GetPlatformStats(
            [FromServices] IRepository<Models.Category, Guid> categoryRepo,
            [FromServices] IGameRepository gameRepo,
            [FromServices] IQuestionRepository questionRepo)
        {
            return Ok(new PlatformStats
            {
                NumberOfCategories = await categoryRepo.CountAsync(),
                NumberOfGamesPlayed = await gameRepo.CountAsync(),
                NumberOfQuestions = await questionRepo.CountAsync(q => q.Status == Models.QuestionStatus.Approved),
                NumberOfContributions = 0 //TODO: Add contributions ability and handle count here
            });
        }

        /// <summary>
        /// Retrieves detailed information on a bygone minigame by id
        /// </summary>
        /// <param name="minigameId">minigame identifier</param>
        /// <returns>Detailed minigame information</returns>
        [HttpGet("Minigame/{minigameId}")]
        [ProducesResponseType(typeof(DetailedMiniGame), StatusCodes.Status200OK)]
        public async Task<ActionResult<DetailedMiniGame>> GetPlatformMinigameById(
            Guid minigameId,
#pragma warning disable CS1573 // no xml comments for service injection
            [FromServices] IMinigameRepository miniGameRepo,
            [FromServices] IMapper mapper)
#pragma warning restore CS1573
        {
            var minigame = await miniGameRepo.GetAsync(minigameId);
            if (minigame == null || minigame.Status == Models.MiniGameStatus.Unknown)
                return NotFound();

            return Ok(mapper.Map<DetailedMiniGame>(minigame));
        }

        /// <summary>
        /// Retrieves a list of all available questions 
        /// </summary>
        /// <returns>list of questions</returns>
        [HttpGet("Question")]
        [ProducesResponseType(typeof(IEnumerable<Question>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Question>>> GetPlatformQuestions(
            [FromServices] IQuestionRepository questionRepo,
            [FromServices] IMapper mapper)
        {
            var questions = await questionRepo.GetAllAsync();
            return Ok(questions.Select(q => mapper.Map<Question>(q)).ToList());
        }

        /// <summary>
        /// Adds a rating for the specified question 
        /// </summary>
        /// <param name="questionId">question identifier</param>
        /// <param name="rating">rating (1-5)</param>
        /// <returns>list of questions</returns>
        [HttpPost("Question/{questionId}/Rating")]
        [ProducesResponseType(typeof(Question), StatusCodes.Status200OK)]
        public async Task<ActionResult<Question>> AddPlatformQuestionRating(
            Guid questionId,
            [Required, Range(1,5)] int rating,
#pragma warning disable CS1573 // no xml comments for service injection
            [FromServices] IQuestionRepository questionRepo,
            [FromServices] DataContext dataContext,
            [FromServices] IMapper mapper)
#pragma warning restore CS1573
        {
            var question = await questionRepo.GetAsync(questionId);
            if (question == null)
                return NotFound();

            question.Ratings.Add(new Models.QuestionRating
            {
                Rating = rating
            });

            await dataContext.SaveChangesAsync();

            return Ok(mapper.Map<Question>(question));
        }

        /// <summary>
        /// Adds the specified question to the question catalogue (with status pending)
        /// </summary>
        /// <param name="question">question to add</param>
        /// <returns>created question</returns>
        [HttpPost("Question")]
        [ProducesResponseType(typeof(Question), StatusCodes.Status201Created)]
        public async Task<ActionResult<Question>> AddPlatformQuestion(
            [FromBody] Question question,
#pragma warning disable CS1573 // no xml comments for service injection
            [FromServices] IQuestionRepository questionRepo,
            [FromServices] IRepository<Models.Category, Guid> categoryRepo,
            [FromServices] DataContext dataContext,
            [FromServices] IMapper mapper)
#pragma warning restore CS1573
        {
            var questionModel = mapper.Map<Models.Question>(question);
            questionModel.GroupId = Guid.NewGuid();
            var category = await categoryRepo.GetAsync(question.Category.Id);
            if (category == null)
                return BadRequest("Unknown category");

            questionModel.Category = category;
            questionModel.Status = Models.QuestionStatus.Pending;

            await questionRepo.AddAsync(questionModel);
            await dataContext.SaveChangesAsync();

            return Created(string.Empty, mapper.Map<Question>(questionModel));            
        }

        /// <summary>
        /// Retrieves a list of all available categories
        /// </summary>
        /// <returns>list of categories</returns>
        [HttpGet("Category")]
        [ProducesResponseType(typeof(IEnumerable<Category>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Category>>> GetPlatformCategories(
            [FromServices] IRepository<Models.Category, Guid> categoryRepo,
            [FromServices] IMapper mapper)
        {
            var categories = await categoryRepo.GetAllAsync();
            return Ok(categories.Select(c => mapper.Map<Category>(c)).ToList());
        }

        /// <summary>
        /// Creates a new report for mistakes/errors within a minigame
        /// </summary>
        /// <param name="report">Report to be created</param>
        /// <returns>Created report</returns>
        [HttpPost("Report")]
        [ProducesResponseType(typeof(Report), StatusCodes.Status201Created)]
        public async Task<ActionResult<Report>> AddPlatformReport(
            [FromBody] Report report,
#pragma warning disable CS1573 // no xml comments for service injection
            [FromServices] IRepository<Models.Report, Guid> reportRepo,
            [FromServices] IMinigameRepository minigameRepo,
            [FromServices] DataContext dataContext,
            [FromServices] IMapper mapper)
#pragma warning restore CS1573
        {
            var reportModel = mapper.Map<Models.Report>(report);
            if (report.MinigameId.HasValue)
            {
                var minigame = await minigameRepo.GetAsync(report.MinigameId.Value);
                if (minigame == null)
                    return BadRequest();
            }

            await reportRepo.AddAsync(reportModel);
            await dataContext.SaveChangesAsync();

            return Created(string.Empty, reportModel);
        }
    }
}