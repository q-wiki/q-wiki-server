using Microsoft.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikidataGame.Models;
using Xunit;

[assembly: CollectionBehavior(CollectionBehavior.CollectionPerClass, DisableTestParallelization = true)]
namespace WikidataGame.ApiClient.Tests
{
    public abstract class ClientTestBase
    {
        public const string BaseUrl = "http://localhost:57635/"; //"https://wikidatagame.azurewebsites.net";


        protected async Task<AuthInfo> RetrieveBearerAsync()
        {
            var apiClient = new WikidataGameAPI(new Uri(BaseUrl), new TokenCredentials("auth"));
            return await apiClient.AuthenticateAsync(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), string.Empty);
        }

        protected async Task<(MiniGameResult Result, string Id)> CreateAndAnswerMinigameRandomly(WikidataGameAPI client, string gameId, Models.Tile tile)
        {
            var minigame = await client.InitalizeMinigameAsync(gameId, new Models.MiniGameInit
            {
                CategoryId = string.IsNullOrEmpty(tile.ChosenCategoryId) ? tile.AvailableCategories.First().Id : tile.ChosenCategoryId,
                TileId = tile.Id
            });
            var answer = new List<string>();
            switch (minigame.Type.Value)
            {
                case 0:
                    answer = minigame.AnswerOptions.ToList();
                    break;
                default:
                    answer.Add(minigame.AnswerOptions.First());
                    break;
            }

            var minigameAnswer = await client.AnswerMinigameAsync(gameId, minigame.Id, answer);
            if (minigameAnswer.CorrectAnswer.SequenceEqual(answer))
            {
                Assert.True(minigameAnswer.IsWin);
            }
            else
            {
                Assert.False(minigameAnswer.IsWin);
            }
            return (minigameAnswer, minigame.Id);
        }
    }
}
