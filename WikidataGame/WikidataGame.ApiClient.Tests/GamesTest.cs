using Microsoft.Rest;
using System;
using System.Linq;
using Xunit;

namespace WikidataGame.ApiClient.Tests
{
    public class GameTest : ClientTestBase
    {
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
            ModelAssertion.AssertGameInfo(gameInfo);

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
            ModelAssertion.AssertGame(game);
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

        [Fact]
        public async void GetGames_NoGamesCreated_ReturnsEmpty()
        {
            var authInfo = await RetrieveBearerAsync();
            var apiClient = new WikidataGameAPI(new Uri(BaseUrl), new TokenCredentials(authInfo.Bearer));
            var games = await apiClient.GetGamesAsync();
            Assert.Empty(games);
        }

        [Fact]
        public async void GetGames_WithCreatedGame_ReturnsGame()
        {
            var authInfo = await RetrieveBearerAsync();
            var apiClient = new WikidataGameAPI(new Uri(BaseUrl), new TokenCredentials(authInfo.Bearer));
            var gameInfo = await apiClient.CreateNewGameAsync();
            var games = await apiClient.GetGamesAsync();
            Assert.NotEmpty(games);
            Assert.All(games, gi => ModelAssertion.AssertGameInfo(gi));
            Assert.True(games.First().GameId == gameInfo.GameId);
        }
    }
}
