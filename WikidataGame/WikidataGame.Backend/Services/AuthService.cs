using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Requests;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Games.v1;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WikidataGame.Backend.Services
{
    public class AuthService
    {
        private readonly GoogleClientSecrets _clientSercrets;
        private readonly HttpClient _httpClient = new HttpClient();
        private const string GoogleOAuthTokenEndpoint = "https://oauth2.googleapis.com/token";

        public AuthService(string googleClientSecret)
        {
            _clientSercrets = GoogleClientSecrets.Load(
                new MemoryStream(
                    Encoding.UTF8.GetBytes(googleClientSecret)
                    )
                );
        }

        public async Task<(bool Success, GoogleResponse Response)> VerifyAuthCodeAsync(string authCode)
        {
            var request = new AuthorizationCodeTokenRequest()
            {
                ClientId = _clientSercrets.Secrets.ClientId,
                ClientSecret = _clientSercrets.Secrets.ClientSecret,
                RedirectUri = "",
                Code = authCode,
                GrantType = "authorization_code"
            };
            try
            {
                var tokenResponse = await request.ExecuteAsync(
                    _httpClient,
                    GoogleOAuthTokenEndpoint,
                    new System.Threading.CancellationToken(),
                    Google.Apis.Util.SystemClock.Default);
                if (tokenResponse == null)
                    return (false, null);

                GoogleCredential credential = GoogleCredential.FromAccessToken(tokenResponse.AccessToken);
                using (GamesService gs = new GamesService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Q-Wiki",
                }))
                {
                    var player = await gs.Players.Get("me").ExecuteAsync();
                    if (player == null)
                        return (false, null);

                    return (true, new GoogleResponse
                    {
                        GooglePlayId = player.PlayerId,
                        GooglePlayAccessToken = tokenResponse.AccessToken,
                        GooglePlayRefreshToken = tokenResponse.RefreshToken,
                        GooglePlayProfileImage = player.AvatarImageUrl
                    });
                }
            }
            catch (TokenResponseException)
            {
                return (false, null);
            }
        }
    }
    public class GoogleResponse
    {
        public string GooglePlayId { get; set; }
        public string GooglePlayAccessToken { get; set; }
        public string GooglePlayRefreshToken { get; set; }
        public string GooglePlayProfileImage { get; set; }
    }
}
