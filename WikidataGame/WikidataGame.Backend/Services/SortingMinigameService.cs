using System;
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
            IQuestionRepository questionRepo,
            DataContext dataContext) : base(minigameRepo, questionRepo, dataContext)
        {
        }

        public Models.MiniGameType MiniGameType => Models.MiniGameType.Sort;

        public MiniGame GenerateMiniGame(string gameId, string playerId, string categoryId)
        {
            throw new NotImplementedException();
        }
    }
}
