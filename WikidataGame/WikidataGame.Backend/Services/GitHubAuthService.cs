using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace WikidataGame.Backend.Services
{
    public class GitHubAuthService
    {
        private const string GitHubOAuthUrl = "https://github.com/login/oauth/access_token";
        private readonly string _gitHubClientId;
        private readonly string _gitHubClientSecret;

        public GitHubAuthService(string gitHubClientId, string gitHubClientSecret)
        {
            _gitHubClientId = gitHubClientId;
            _gitHubClientSecret = gitHubClientSecret;
        }

        public async Task<string> RetrieveGitHubBearerTokenAsync(string accessToken)
        {
            var requestBody = new Dictionary<string, string>
            {
                { "client_id", _gitHubClientId },
                { "client_secret", _gitHubClientSecret },
                { "code", accessToken }
            };

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var req = new HttpRequestMessage(HttpMethod.Post, GitHubOAuthUrl)
                {
                    Content = new FormUrlEncodedContent(requestBody),
                };
                var res = await client.SendAsync(req);
                res.EnsureSuccessStatusCode();
                var jsonResponse = await res.Content.ReadAsStringAsync();
                var tokenInfo = JsonConvert.DeserializeObject<TokenInformation>(jsonResponse);
                return tokenInfo.AccessToken;
            }
        }

    }

    public class TokenInformation
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }
    }

}
