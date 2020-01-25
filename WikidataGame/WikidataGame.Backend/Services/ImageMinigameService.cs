using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WikidataGame.Backend.Models;
using WikidataGame.Backend.Helpers;
using WikidataGame.Backend.Repos;
using System.Text.RegularExpressions;

namespace WikidataGame.Backend.Services
{
    public class ImageMinigameService : MinigameServiceBase, IMinigameService
    {
        public ImageMinigameService(
            IMinigameRepository minigameRepo,
            DataContext dataContext) : base(minigameRepo, dataContext)
        {
        }

        public MiniGameType MiniGameType => MiniGameType.Image;

        public async Task<MiniGame> GenerateMiniGameAsync(Guid gameId, Guid playerId, Question question, Guid tileId)
        {
            // use method in baseclass to query wikidata with question
            var data = QueryWikidata(question.SparqlQuery);

            var minigame = await _minigameRepo.CreateMiniGameAsync(gameId, playerId, tileId, question, MiniGameType);
            minigame.TaskDescription = question.TaskDescription;
            minigame.CorrectAnswer = new List<string> { data[0].Item2 }; // placeholder and answer in first tuple!
            var templist = data.Select(item => item.Item2).ToList();
            minigame.AnswerOptions = templist.OrderBy(a => Guid.NewGuid()).ToList(); // shuffle answer options

            (var thumbUrl, var licenseString) = await CommonsImageService.RetrieveImageInfoStringByUrlAsync(data[0].Item1);
            minigame.LicenseInfo = licenseString;
            minigame.ImageUrl = thumbUrl;
            await _dataContext.SaveChangesAsync();

            return minigame;
        }

    }
}
