using Microsoft.Rest;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using WikidataGame.Models;
using Xunit;

namespace WikidataGame.ApiClient.Tests
{
    public class AuthTest : ClientTestBase
    {
        [Fact]
        public async void RequestAuth_ForTestUser_ReturnsBearer()
        {
            var apiClient = new WikidataGameAPI(new Uri(BaseUrl), new TokenCredentials("auth"));
            var authInfo = await apiClient.AuthenticateAsync(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), string.Empty);
            AssertAuthInfo(authInfo);
        }

        [Fact]
        public async void RequestNewAuth_AfterAccountCreation_ReturnsBearer()
        {
            var password = Guid.NewGuid().ToString();
            var apiClient = new WikidataGameAPI(new Uri(BaseUrl), new TokenCredentials("auth"));
            var authInfo = await apiClient.AuthenticateAsync(Guid.NewGuid().ToString(), password, string.Empty);
            var apiClient2 = new WikidataGameAPI(new Uri(BaseUrl), new TokenCredentials("auth"));
            var authInfo2 = await apiClient2.AuthenticateAsync(authInfo.User.Name, password, string.Empty);
            AssertAuthInfo(authInfo2);
        }

        [Fact]
        public async void RequestAuth_WithWrongCredentials_ThrowsHttpOperationEx()
        {
            var ex = await Assert.ThrowsAsync<HttpOperationException>(async () =>
            {
                var authInfo = await RetrieveBearerAsync();
                var apiClient = new WikidataGameAPI(new Uri(BaseUrl), new TokenCredentials("auth"));
                var authInfo2 = await apiClient.AuthenticateAsync(authInfo.User.Name, Guid.NewGuid().ToString(), string.Empty);
            });

            Assert.True(ex.Response.StatusCode == HttpStatusCode.Unauthorized);
        }


        [Fact]
        public async void RequestAuth_TooShortPassword_ThrowsHttpOperationEx()
        {
            var ex = await Assert.ThrowsAsync<HttpOperationException>(async () =>
            {
                var apiClient = new WikidataGameAPI(new Uri(BaseUrl), new TokenCredentials("auth"));
                var authInfo2 = await apiClient.AuthenticateAsync(Guid.NewGuid().ToString(), "123", string.Empty);
            });

            Assert.True(ex.Response.StatusCode == HttpStatusCode.BadRequest);
        }

        [Fact]
        public async void RequestAuth_TooShortUsername_ThrowsHttpOperationEx()
        {
            var ex = await Assert.ThrowsAsync<HttpOperationException>(async () => {
                var apiClient = new WikidataGameAPI(new Uri(BaseUrl), new TokenCredentials("auth"));
                await apiClient.AuthenticateAsync(Guid.NewGuid().ToString().Substring(0, 2), Guid.NewGuid().ToString(), string.Empty);
            });

            Assert.True(ex.Response.StatusCode == HttpStatusCode.BadRequest);
        }

        private void AssertAuthInfo(AuthInfo authInfo)
        {
            Assert.NotNull(authInfo);
            Assert.False(string.IsNullOrWhiteSpace(authInfo.Bearer));
            Assert.NotNull(authInfo.User);
        }
    }
}
