using Microsoft.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace WikidataGame.ApiClient.Tests
{
    public class MinigameTest : ClientTestBase
    {
        [Fact]
        public async void CreateMinigame_ForGame_ReturnsMinigame()
        {
            var authInfo = await RetrieveBearerAsync();
            var apiClient = new WikidataGameAPI(new Uri(BaseUrl), new TokenCredentials(authInfo.Bearer));
            var gameInfo = await apiClient.CreateNewGameAsync();
            var game = await apiClient.RetrieveGameStateAsync(gameInfo.GameId);
            if (game.AwaitingOpponentToJoin.Value)
            {
                var authInfo2 = await RetrieveBearerAsync();
                apiClient = new WikidataGameAPI(new Uri(BaseUrl), new TokenCredentials(authInfo2.Bearer));
                var gameInfo2 = await apiClient.CreateNewGameAsync();
                game = await apiClient.RetrieveGameStateAsync(gameInfo2.GameId);
            }

            Assert.False(game.AwaitingOpponentToJoin.Value);
            var tile = game.Tiles.Where(cg => cg.Count(c => c != null) > 0).First().Where(c => c != null).First();
            var minigame = await apiClient.InitalizeMinigameAsync(game.Id, new Models.MiniGameInit
            {
                CategoryId = tile.AvailableCategories.First().Id,
                TileId = tile.Id
            });

            Assert.False(string.IsNullOrWhiteSpace(minigame.Id));
            Assert.InRange(minigame.AnswerOptions.Count, 4, 4);
            foreach(var option in minigame.AnswerOptions)
            {
                Assert.False(string.IsNullOrWhiteSpace(option));
            }
            Assert.False(string.IsNullOrWhiteSpace(minigame.TaskDescription));
            Assert.NotNull(minigame.Type);

            //cleanup
            await apiClient.DeleteGameAsync(gameInfo.GameId);
        }
    }
}
