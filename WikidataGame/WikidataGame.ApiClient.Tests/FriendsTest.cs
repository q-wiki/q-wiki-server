using Microsoft.Rest;
using System;
using System.Linq;
using Xunit;

namespace WikidataGame.ApiClient.Tests
{
    public class FriendsTest : ClientTestBase
    {
        [Fact]
        public async void RequestFriendList_ForTestUser_ReturnsEmptyList()
        {
            var authInfo = await RetrieveBearerAsync();
            var apiClient = new WikidataGameAPI(new Uri(BaseUrl), new TokenCredentials(authInfo.Bearer));
            var friends = await apiClient.GetFriendsAsync();
            Assert.Empty(friends);
        }

        [Fact]
        public async void AddFriend_ForTestUser_FriendHasBeenAdded()
        {
            var authInfo = await RetrieveBearerAsync();
            var apiClient = new WikidataGameAPI(new Uri(BaseUrl), new TokenCredentials(authInfo.Bearer));
            var authInfo2 = await RetrieveBearerAsync();

            await apiClient.PostFriendAsync(authInfo2.User.Id);

            var friends = await apiClient.GetFriendsAsync();
            Assert.NotEmpty(friends);
            Assert.True(friends.First().Id == authInfo2.User.Id);
        }

        [Fact]
        public async void FindFriend_ForTestUser_FoundFriend()
        {
            var authInfo = await RetrieveBearerAsync();
            var apiClient = new WikidataGameAPI(new Uri(BaseUrl), new TokenCredentials(authInfo.Bearer));
            var authInfo2 = await RetrieveBearerAsync();

            var results = await apiClient.GetFindFriendsAsync(authInfo2.User.Name);

            Assert.NotEmpty(results);
            Assert.True(results.First().Id == authInfo2.User.Id);
        }


        [Fact]
        public async void DeleteFriend_ForTestUser_FriendHasBeenDeleted()
        {
            var authInfo = await RetrieveBearerAsync();
            var apiClient = new WikidataGameAPI(new Uri(BaseUrl), new TokenCredentials(authInfo.Bearer));
            var authInfo2 = await RetrieveBearerAsync();

            await apiClient.PostFriendAsync(authInfo2.User.Id);
            await apiClient.DeleteFriendAsync(authInfo2.User.Id);

            var friends = await apiClient.GetFriendsAsync();
            Assert.Empty(friends);
        }
    }
}
