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
            Assert.Empty(gameRequests);
        }

        [Fact]
        public async void SendGameRequest_ForTestUser_HasBeenCreated()
        {
            var authInfo = await RetrieveBearerAsync();
            var apiClient = new WikidataGameAPI(new Uri(BaseUrl), new TokenCredentials(authInfo.Bearer));
            var authInfo2 = await RetrieveBearerAsync();

            var request = await apiClient.RequestMatchAsync(authInfo2.User.Id);

            var gameRequests = await apiClient.GetGameRequestsAsync();
            Assert.NotEmpty(gameRequests);
            Assert.True(gameRequests.First().Id == request.Id);

            var outgoingGameRequests = await apiClient.GetOutgoingGameRequestsAsync();
            Assert.NotEmpty(outgoingGameRequests);
            Assert.True(outgoingGameRequests.First().Id == request.Id);

            var apiClient2 = new WikidataGameAPI(new Uri(BaseUrl), new TokenCredentials(authInfo2.Bearer));
            var gameRequests2 = await apiClient2.GetGameRequestsAsync();
            Assert.NotEmpty(gameRequests2);
            Assert.True(gameRequests2.First().Id == request.Id);

            var incomingGameRequests = await apiClient2.GetIncomingGameRequestsAsync();
            Assert.NotEmpty(incomingGameRequests);
            Assert.True(incomingGameRequests.First().Id == request.Id);
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
            Assert.Empty(gameRequests);
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

            Assert.False(gameInfo.GameId.HasValue && gameInfo.GameId.Value == default);

            //cleanup
            await apiClient2.DeleteGameAsync(gameInfo.GameId.Value);
        }

    }
}
