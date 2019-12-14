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
            Assert.True(gameRequests.Outgoing.First().Id == request.Id);

            var apiClient2 = new WikidataGameAPI(new Uri(BaseUrl), new TokenCredentials(authInfo2.Bearer));
            var gameRequests2 = await apiClient2.GetGameRequestsAsync();
            Assert.NotEmpty(gameRequests2.Incoming);
            Assert.True(gameRequests2.Incoming.First().Id == request.Id);
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

            Assert.False(string.IsNullOrEmpty(gameInfo.GameId));

            //cleanup
            await apiClient2.DeleteGameAsync(gameInfo.GameId);
        }

    }
}
