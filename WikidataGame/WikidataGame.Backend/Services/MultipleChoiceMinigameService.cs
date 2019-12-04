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
            DataContext dataContext) : base(minigameRepo, dataContext)
        {
        }

        public Models.MiniGameType MiniGameType => Models.MiniGameType.MultipleChoice;

        public async Task<MiniGame> GenerateMiniGameAsync(Guid gameId, Guid playerId, Models.Question question, Guid tileId)
        {
            // use method in baseclass to query wikidata with question
            var data = QueryWikidata(question.SparqlQuery); 

            var minigame = await _minigameRepo.CreateMiniGameAsync(gameId, playerId, tileId, question.CategoryId, MiniGameType);

            minigame.TaskDescription = string.Format(question.TaskDescription, data[0].Item1); // placeholder and answer in first tuple!
            minigame.CorrectAnswer = new List<string> { data[0].Item2 }; // placeholder and answer in first tuple!
            minigame.AnswerOptions = data.Select(item => item.Item2).OrderBy(a => Guid.NewGuid()).ToList(); // shuffle answer options

            await _dataContext.SaveChangesAsync();

            return MiniGame.FromModel(minigame);
        }
    }
}
