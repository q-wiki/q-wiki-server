using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WikidataGame.Backend.Dto;
using WikidataGame.Backend.Helpers;
using WikidataGame.Backend.Repos;

namespace WikidataGame.Backend.Services
{
    public abstract class MinigameServiceBase
    {
        protected readonly IMinigameRepository _minigameRepo;
        protected readonly IQuestionRepository _questionRepo;
        protected readonly DataContext _dataContext;

        public MinigameServiceBase(
            IMinigameRepository minigameRepo,
            IQuestionRepository questionRepo,
            DataContext dataContext)
        {
            _minigameRepo = minigameRepo;
            _questionRepo = questionRepo;
            _dataContext = dataContext;
        }
        //TODO: Add helper methods that communicate with Wikidata


        public bool IsMiniGameAnswerCorrect(Models.MiniGame miniGame, IEnumerable<string> answers)
        {
            return answers.SequenceEqual(miniGame.CorrectAnswer);
        }
    }
}
