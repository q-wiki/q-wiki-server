using Microsoft.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace WikidataGame.ApiClient.Tests
{
    public class MinigameTest : ClientTestBase
    {
        [Fact]
        public async void CreateMinigame_ForFirstAvailableTileAndCategory_ReturnsMinigame()
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
                CategoryId = string.IsNullOrEmpty(tile.ChosenCategoryId) ? tile.AvailableCategories.First().Id : tile.ChosenCategoryId,
                TileId = tile.Id
            });

            ModelAssertion.AssertMinigame(minigame);

            //cleanup
            await apiClient.DeleteGameAsync(gameInfo.GameId);
        }

        [Fact]
        public async void AnswerMinigames_ForDifferentOptions_ReturnsUpdatedTile()
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

            var flattenedTiles = game.Tiles.SelectMany(t => t).ToList();

            //Level up
            var tile = flattenedTiles.SingleOrDefault(t => t != null && t.OwnerId == game.Me.Id);
            var (minigameAnswer, id) = await CreateAndAnswerMinigameRandomly(apiClient, game.Id, tile);
            ModelAssertion.AssertMinigameResult(minigameAnswer);
            var updatedTile = minigameAnswer.Tiles.SelectMany(t => t).SingleOrDefault(t => t != null && t.Id == tile.Id);
            if (minigameAnswer.IsWin.Value)
            {
                Assert.True(updatedTile.Difficulty.Value == tile.Difficulty + 1);
            }
            else
            {
                Assert.True(updatedTile.Difficulty.Value == tile.Difficulty);
            }

            //occupy new tile
            tile = flattenedTiles.Where(t => t != null && string.IsNullOrEmpty(t.OwnerId)).First();
            var (minigameAnswer2, id2) = await CreateAndAnswerMinigameRandomly(apiClient, game.Id, tile);
            ModelAssertion.AssertMinigameResult(minigameAnswer2);
            var updatedTile2 = minigameAnswer2.Tiles.SelectMany(t => t).SingleOrDefault(t => t != null && t.Id == tile.Id);
            if (minigameAnswer2.IsWin.Value)
            {
                Assert.True(updatedTile2.OwnerId == game.Me.Id);
            }
            else
            {
                Assert.True(string.IsNullOrEmpty(updatedTile2.OwnerId));
            }

            //attack opponent
            tile = flattenedTiles.SingleOrDefault(t => t != null && t.OwnerId == game.Opponent.Id);
            var (minigameAnswer3, id3) = await CreateAndAnswerMinigameRandomly(apiClient, game.Id, tile);
            ModelAssertion.AssertMinigameResult(minigameAnswer3);
            var updatedTile3 = minigameAnswer3.Tiles.SelectMany(t => t).SingleOrDefault(t => t != null && t.Id == tile.Id);
            if (minigameAnswer3.IsWin.Value)
            {
                Assert.True(updatedTile3.OwnerId == game.Me.Id);
            }
            else
            {
                Assert.True(updatedTile3.OwnerId == game.Opponent.Id);
            }
            Assert.True(minigameAnswer3.NextMovePlayerId == game.Opponent.Id);

            //cleanup
            await apiClient.DeleteGameAsync(gameInfo.GameId);
        }

        
    }
}
