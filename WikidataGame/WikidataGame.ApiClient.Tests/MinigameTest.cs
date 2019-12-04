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
            var game = await apiClient.RetrieveGameStateAsync(gameInfo.GameId.Value);
            if (game.AwaitingOpponentToJoin.Value)
            {
                var authInfo2 = await RetrieveBearerAsync();
                apiClient = new WikidataGameAPI(new Uri(BaseUrl), new TokenCredentials(authInfo2.Bearer));
                var gameInfo2 = await apiClient.CreateNewGameAsync();
                game = await apiClient.RetrieveGameStateAsync(gameInfo2.GameId.Value);
            }

            Assert.False(game.AwaitingOpponentToJoin.Value);
            var tile = game.Tiles.Where(cg => cg.Count(c => c != null) > 0).First().Where(c => c != null).First();
            var minigame = await apiClient.InitalizeMinigameAsync(game.Id.Value, new Models.MiniGameInit
            {
                CategoryId = !tile.ChosenCategoryId.HasValue ? tile.AvailableCategories.First().Id : tile.ChosenCategoryId,
                TileId = tile.Id
            });

            Assert.True(minigame.Id.HasValue);
            Assert.InRange(minigame.AnswerOptions.Count, 4, 4);
            foreach(var option in minigame.AnswerOptions)
            {
                Assert.False(string.IsNullOrWhiteSpace(option));
            }
            Assert.False(string.IsNullOrWhiteSpace(minigame.TaskDescription));
            Assert.NotNull(minigame.Type);

            //cleanup
            await apiClient.DeleteGameAsync(gameInfo.GameId.Value);
        }

        [Fact]
        public async void AnswerMinigames_ForDifferentOptions_ReturnsUpdatedTile()
        {
            var authInfo = await RetrieveBearerAsync();
            var apiClient = new WikidataGameAPI(new Uri(BaseUrl), new TokenCredentials(authInfo.Bearer));
            var gameInfo = await apiClient.CreateNewGameAsync();
            var game = await apiClient.RetrieveGameStateAsync(gameInfo.GameId.Value);
            if (game.AwaitingOpponentToJoin.Value)
            {
                var authInfo2 = await RetrieveBearerAsync();
                apiClient = new WikidataGameAPI(new Uri(BaseUrl), new TokenCredentials(authInfo2.Bearer));
                var gameInfo2 = await apiClient.CreateNewGameAsync();
                game = await apiClient.RetrieveGameStateAsync(gameInfo2.GameId.Value);
            }

            var flattenedTiles = game.Tiles.SelectMany(t => t).ToList();

            //Level up
            var tile = flattenedTiles.SingleOrDefault(t => t != null && t.OwnerId == game.Me.Id);
            var minigameAnswer = await CreateAndAnswerMinigameRandomly(apiClient, game.Id.Value, tile);
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
            tile = flattenedTiles.Where(t => t != null && !t.OwnerId.HasValue).First();
            minigameAnswer = await CreateAndAnswerMinigameRandomly(apiClient, game.Id.Value, tile);
            updatedTile = minigameAnswer.Tiles.SelectMany(t => t).SingleOrDefault(t => t != null && t.Id == tile.Id);
            if (minigameAnswer.IsWin.Value)
            {
                Assert.True(updatedTile.OwnerId == game.Me.Id);
            }
            else
            {
                Assert.False(updatedTile.OwnerId.HasValue);
            }

            //attack opponent
            tile = flattenedTiles.SingleOrDefault(t => t != null && t.OwnerId == game.Opponent.Id);
            minigameAnswer = await CreateAndAnswerMinigameRandomly(apiClient, game.Id.Value, tile);
            updatedTile = minigameAnswer.Tiles.SelectMany(t => t).SingleOrDefault(t => t != null && t.Id == tile.Id);
            if (minigameAnswer.IsWin.Value)
            {
                Assert.True(updatedTile.OwnerId == game.Me.Id);
            }
            else
            {
                Assert.True(updatedTile.OwnerId == game.Opponent.Id);
            }
            Assert.True(minigameAnswer.NextMovePlayerId == game.Opponent.Id);

            //cleanup
            await apiClient.DeleteGameAsync(gameInfo.GameId.Value);
        }

        private async Task<Models.MiniGameResult> CreateAndAnswerMinigameRandomly(WikidataGameAPI client, Guid gameId, Models.Tile tile)
        {
            var minigame = await client.InitalizeMinigameAsync(gameId, new Models.MiniGameInit
            {
                CategoryId = !tile.ChosenCategoryId.HasValue ? tile.AvailableCategories.First().Id : tile.ChosenCategoryId,
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

            var minigameAnswer = await client.AnswerMinigameAsync(gameId, minigame.Id.Value, answer);
            if (minigameAnswer.CorrectAnswer.SequenceEqual(answer))
            {
                Assert.True(minigameAnswer.IsWin);
            }
            else
            {
                Assert.False(minigameAnswer.IsWin);
            }
            Assert.NotEmpty(minigameAnswer.Tiles);
            Assert.NotEmpty(minigameAnswer.CorrectAnswer);
            Assert.True(minigameAnswer.NextMovePlayerId.HasValue);
            return minigameAnswer;
        }
    }
}
