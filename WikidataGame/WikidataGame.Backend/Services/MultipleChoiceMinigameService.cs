using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WikidataGame.Backend.Dto;
using WikidataGame.Backend.Helpers;
using WikidataGame.Backend.Repos;

namespace WikidataGame.Backend.Services
{
    public class MultipleChoiceMinigameService : MinigameServiceBase, IMinigameService
    {
        public MultipleChoiceMinigameService(
            IMinigameRepository minigameRepo,
            IQuestionRepository questionRepo,
            DataContext dataContext) : base(minigameRepo, questionRepo, dataContext)
        {
        }

        public Models.MiniGameType MiniGameType => Models.MiniGameType.MultipleChoice;

        public MiniGame GenerateMiniGame(string gameId, string playerId, string categoryId)
        {
            var question = _questionRepo.GetRandomQuestionForMinigameType(MiniGameType, categoryId);
            //TODO: use method in baseclass to query wikidata with question

            var minigame = _minigameRepo.CreateMiniGame(gameId, playerId, MiniGameType);

            minigame.TaskDescription = string.Format(question.TaskDescription, "{placefolder fill}"); //TODO: Add placeholder data from wikidata
            minigame.AnswerOptions = new List<string> { "", "", "", "" }; //TODO: Add answer options from wikidata
            minigame.CorrectAnswer = new List<string> { "" }; //TODO: Add correct answer from wikidata

            _dataContext.SaveChanges();

            return MiniGame.FromModel(minigame);
        }
    }
}
