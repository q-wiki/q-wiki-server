using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WikidataGame.Backend.Dto;
using WikidataGame.Backend.Helpers;
using WikidataGame.Backend.Repos;

namespace WikidataGame.Backend.Services
{
    public class BlurryImageMinigameService : MinigameServiceBase, IMinigameService
    {
        public BlurryImageMinigameService(
            IMinigameRepository minigameRepo,
            IQuestionRepository questionRepo,
            DataContext dataContext) : base(minigameRepo, questionRepo, dataContext)
        {
        }

        public Models.MiniGameType MiniGameType => Models.MiniGameType.BlurryImage;

        public Dto.MiniGame GenerateMiniGame(string gameId, string playerId)
        {
            throw new NotImplementedException();
        }
    }
}
