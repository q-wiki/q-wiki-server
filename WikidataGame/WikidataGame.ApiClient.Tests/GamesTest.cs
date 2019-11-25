using Microsoft.Rest;
using System;
using Xunit;

namespace WikidataGame.ApiClient.Tests
{
    public class GameTest : ClientTestBase
    {
        [Fact]
        public async void RequestAuth_ForTestUser_ReturnsBearer()
        {
            var authInfo = await RetrieveBearerAsync();
            Assert.False(string.IsNullOrWhiteSpace(authInfo.Bearer));
            Assert.NotNull(authInfo.Expires);
        }

        [Fact]
        public async void CreateGame_WithWrongCredentials_ReturnsNull()
        {
            var apiClient = new WikidataGameAPI(new Uri(BaseUrl), new TokenCredentials("bla"));
            var gameInfo = await apiClient.CreateNewGameAsync();
            Assert.Null(gameInfo);
        }

        [Fact]
        public async void CreateGame_ForNewUser_ReturnsCreatedGame()
        {
            var authInfo = await RetrieveBearerAsync();
            var apiClient = new WikidataGameAPI(new Uri(BaseUrl), new TokenCredentials(authInfo.Bearer));
            var gameInfo = await apiClient.CreateNewGameAsync();
            Assert.False(string.IsNullOrWhiteSpace(gameInfo.GameId));

            //cleanup
            await apiClient.DeleteGameAsync(gameInfo.GameId);
        }

        [Fact]
        public async void RetrieveGame_WithGameId_ReturnsGame()
        {
            var authInfo = await RetrieveBearerAsync();
            var apiClient = new WikidataGameAPI(new Uri(BaseUrl), new TokenCredentials(authInfo.Bearer));
            var gameInfo = await apiClient.CreateNewGameAsync();
            var game = await apiClient.RetrieveGameStateAsync(gameInfo.GameId);
            Assert.False(string.IsNullOrWhiteSpace(game.Id));
            Assert.NotNull(game.AwaitingOpponentToJoin);
            Assert.NotNull(game.Me);
            Assert.False(string.IsNullOrWhiteSpace(game.Me.Id));
            Assert.False(string.IsNullOrWhiteSpace(game.Me.Name));
            if (!game.AwaitingOpponentToJoin.Value)
            {
                Assert.NotNull(game.Opponent);
                Assert.False(string.IsNullOrWhiteSpace(game.Opponent.Id));
                Assert.False(string.IsNullOrWhiteSpace(game.Opponent.Name));
            }
            Assert.NotEmpty(game.Tiles);
            foreach(var tilegroup in game.Tiles)
            {
                foreach (var tile in tilegroup)
                {
                    if (tile != null)
                    {
                        Assert.False(string.IsNullOrWhiteSpace(tile.Id));
                        Assert.NotNull(tile.Difficulty);
                        Assert.NotEmpty(tile.AvailableCategories);
                        Assert.Null(tile.ChosenCategoryId);
                    }
                }
            }
            Assert.Empty(game.WinningPlayerIds);

            //cleanup
            await apiClient.DeleteGameAsync(gameInfo.GameId);
        }

        [Fact]
        public async void RetrieveGame_WithInvalidGameId_ReturnsNull()
        {
            var authInfo = await RetrieveBearerAsync();
            var apiClient = new WikidataGameAPI(new Uri(BaseUrl), new TokenCredentials(authInfo.Bearer));
            var game = await apiClient.RetrieveGameStateAsync(Guid.NewGuid().ToString());
            Assert.Null(game);
        }

        [Fact]
        public async void DeleteGame_WithGameId_Succeeds()
        {
            var authInfo = await RetrieveBearerAsync();
            var apiClient = new WikidataGameAPI(new Uri(BaseUrl), new TokenCredentials(authInfo.Bearer));
            var gameInfo = await apiClient.CreateNewGameAsync();
            await apiClient.DeleteGameAsync(gameInfo.GameId);
            var game = await apiClient.RetrieveGameStateAsync(gameInfo.GameId);
            Assert.Null(game);
        }
    }
}
