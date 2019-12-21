using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WikidataGame.Backend.Dto;
using WikidataGame.Backend.Helpers;
using WikidataGame.Backend.Repos;

namespace WikidataGame.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IGameRepository _gameRepo;
        private readonly IRepository<Models.Category, Guid> _categoryRepo;
        private readonly IQuestionRepository _questionRepo;
        private readonly IMinigameRepository _miniGameRepo;
        private readonly IMapper _mapper;

        public PlatformController(
            DataContext dataContext,
            IGameRepository gameRepo,
            IRepository<Models.Category, Guid> categoryRepo,
            IQuestionRepository questionRepo,
            IMinigameRepository miniGameRepo,
            IMapper mapper)
        {
            _dataContext = dataContext;
            _gameRepo = gameRepo;
            _categoryRepo = categoryRepo;
            _questionRepo = questionRepo;
            _miniGameRepo = miniGameRepo;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves statistics containing the number of categories, games played and questions added
        /// </summary>
        /// <returns>Current statistics</returns>
        [HttpGet("Stats")]
        [ProducesResponseType(typeof(PlatformStats), StatusCodes.Status200OK)]
        public async Task<ActionResult<PlatformStats>> GetPlatformStats()
        {
            return Ok(new PlatformStats
            {
                NumberOfCategories = await _categoryRepo.CountAsync(),
                NumberOfGamesPlayed = await _gameRepo.CountAsync(),
                NumberOfQuestions = await _questionRepo.CountAsync(q => q.Status == Models.QuestionStatus.Approved),
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
        public async Task<ActionResult<DetailedMiniGame>> GetPlatformMinigameById(Guid minigameId)
        {
            var minigame = await _miniGameRepo.GetAsync(minigameId);
            if (minigame == null || minigame.Status == Models.MiniGameStatus.Unknown)
                return NotFound();

            return Ok(_mapper.Map<DetailedMiniGame>(minigame));
        }

        /// <summary>
        /// Retrieves a list of all available questions 
        /// </summary>
        /// <returns>list of questions</returns>
        [HttpGet("Question")]
        [ProducesResponseType(typeof(IEnumerable<Question>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Question>>> GetPlatformQuestions()
        {
            var questions = await _questionRepo.GetAllAsync();
            return Ok(questions.Select(q => _mapper.Map<Question>(q)).ToList());
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
            [Required, Range(1,5)] int rating)
        {
            var question = await _questionRepo.GetAsync(questionId);
            if (question == null)
                return NotFound();

            question.Ratings.Add(new Models.QuestionRating
            {
                Rating = rating
            });

            await _dataContext.SaveChangesAsync();

            return Ok(_mapper.Map<Question>(question));
        }

        [HttpPost("Question")]
        [ProducesResponseType(typeof(Question), StatusCodes.Status201Created)]
        public async Task<ActionResult<Question>> AddPlatformQuestion([FromBody] Question question)
        {
            return null;
        }

        /// <summary>
        /// Retrieves a list of all available categories
        /// </summary>
        /// <returns>list of categories</returns>
        [HttpGet("Categories")]
        [ProducesResponseType(typeof(IEnumerable<Category>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Category>>> GetPlatformCategories()
        {
            var categories = await _categoryRepo.GetAllAsync();
            return Ok(categories.Select(c => _mapper.Map<Category>(c)).ToList());
        }


    }
}