﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WikidataGame.Backend.Dto;
using WikidataGame.Backend.Helpers;
using WikidataGame.Backend.Repos;

namespace WikidataGame.Backend.Services
{
    public class SortingMinigameService : MinigameServiceBase, IMinigameService
    {
        public SortingMinigameService(
            IMinigameRepository minigameRepo,
            DataContext dataContext) : base(minigameRepo, dataContext)
        {
        }

        public Models.MiniGameType MiniGameType => Models.MiniGameType.Sort;

        public MiniGame GenerateMiniGame(string gameId, string playerId, Models.Question question, string tileId)
        {
            // use method in baseclass to query wikidata with question
            var data = QueryWikidata(question.SparqlQuery);

            var minigame = _minigameRepo.CreateMiniGame(gameId, playerId, tileId, question.CategoryId, MiniGameType);

            // each row looks like this: (placeholderValue, label) where placeholderValue is the value
            // that gets used in the template string, value is the actual value to sort on and label is the
            // option label presented to the user.
            //
            // NOTE: The actual value can be present in the result but will be ignored!
            minigame.TaskDescription = string.Format(question.TaskDescription, data[0].Item1); // placeholder and answer in first tuple!
            minigame.CorrectAnswer = data.Select(d => d.Item2).ToList(); // placeholder and answer in first tuple!
            minigame.AnswerOptions = data.Select(d => d.Item2).OrderBy(_ => Guid.NewGuid()).ToList(); // shuffle answer options

            _dataContext.SaveChanges();

            return MiniGame.FromModel(minigame);
        }
    }
}
