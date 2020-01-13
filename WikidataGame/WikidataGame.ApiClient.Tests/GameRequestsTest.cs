using Microsoft.Rest;
using System;
using System.Linq;
using Xunit;

namespace WikidataGame.ApiClient.Tests
{
    public class GameRequestsTest : ClientTestBase
    {
        [Fact]
        public async void RequestGameRequests_ForTestUser_ReturnsEmptyList()
        {
            var authInfo = await RetrieveBearerAsync();
            var apiClient = new WikidataGameAPI(new Uri(BaseUrl), new TokenCredentials(authInfo.Bearer));
            var gameRequests = await apiClient.GetGameRequestsAsync();
            Assert.Empty(gameRequests.Outgoing);
            Assert.Empty(gameRequests.Incoming);
        }

        [Fact]
        public async void SendGameRequest_ForTestUser_HasBeenCreated()
        {
            var authInfo = await RetrieveBearerAsync();
            var apiClient = new WikidataGameAPI(new Uri(BaseUrl), new TokenCredentials(authInfo.Bearer));
            var authInfo2 = await RetrieveBearerAsync();

            var request = await apiClient.RequestMatchAsync(authInfo2.User.Id);

            var gameRequests = await apiClient.GetGameRequestsAsync();
            Assert.NotEmpty(gameRequests.Outgoing);
            Assert.All(gameRequests.Outgoing, gr => ModelAssertion.AssertGameRequest(gr));
            Assert.True(gameRequests.Outgoing.First().Id == request.Id);

            var apiClient2 = new WikidataGameAPI(new Uri(BaseUrl), new TokenCredentials(authInfo2.Bearer));
            var gameRequests2 = await apiClient2.GetGameRequestsAsync();
            Assert.NotEmpty(gameRequests2.Incoming);
            Assert.All(gameRequests.Incoming, gr => ModelAssertion.AssertGameRequest(gr));
            Assert.True(gameRequests2.Incoming.First().Id == request.Id);
        }

        [Fact]
        public async void SendMultipleGameRequests_ForTestUser_RequestsHaveBeenCreated()
        {
            var authInfo = await RetrieveBearerAsync();
            var apiClient = new WikidataGameAPI(new Uri(BaseUrl), new TokenCredentials(authInfo.Bearer));
            var authInfo2 = await RetrieveBearerAsync();

            var request = await apiClient.RequestMatchAsync(authInfo2.User.Id);

            var gameRequests = await apiClient.GetGameRequestsAsync();
            Assert.NotEmpty(gameRequests.Outgoing);
            Assert.Equal(1, gameRequests.Outgoing.Count);
            Assert.All(gameRequests.Outgoing, gr => ModelAssertion.AssertGameRequest(gr));

            var authInfo3 = await RetrieveBearerAsync();

            request = await apiClient.RequestMatchAsync(authInfo3.User.Id);

            gameRequests = await apiClient.GetGameRequestsAsync();
            Assert.NotEmpty(gameRequests.Outgoing);
            Assert.Equal(2, gameRequests.Outgoing.Count);
            Assert.All(gameRequests.Outgoing, gr => ModelAssertion.AssertGameRequest(gr));
        }

        [Fact]
        public async void DeleteGameRequest_ForTestUser_GameRequestHasBeenDeleted()
        {
            var authInfo = await RetrieveBearerAsync();
            var apiClient = new WikidataGameAPI(new Uri(BaseUrl), new TokenCredentials(authInfo.Bearer));
            var authInfo2 = await RetrieveBearerAsync();

            var request = await apiClient.RequestMatchAsync(authInfo2.User.Id);
            await apiClient.DeleteGameRequestAsync(request.Id);

            var gameRequests = await apiClient.GetGameRequestsAsync();
            Assert.Empty(gameRequests.Outgoing);
        }

        [Fact]
        public async void AcceptGameRequest_ForTestUser_GameCreated()
        {
            var authInfo = await RetrieveBearerAsync();
            var apiClient = new WikidataGameAPI(new Uri(BaseUrl), new TokenCredentials(authInfo.Bearer));
            var authInfo2 = await RetrieveBearerAsync();

            var request = await apiClient.RequestMatchAsync(authInfo2.User.Id);
            var apiClient2 = new WikidataGameAPI(new Uri(BaseUrl), new TokenCredentials(authInfo2.Bearer));
            var gameInfo = await apiClient2.CreateNewGameByRequestAsync(request.Id);

            ModelAssertion.AssertGameInfo(gameInfo);
            ModelAssertion.AssertPlayer(gameInfo.Opponent);
            Assert.Equal(gameInfo.Opponent.Id, authInfo.User.Id);
            Assert.False(string.IsNullOrEmpty(gameInfo.NextMovePlayerId));

            //cleanup
            await apiClient2.DeleteGameAsync(gameInfo.GameId);
        }

    }
}
