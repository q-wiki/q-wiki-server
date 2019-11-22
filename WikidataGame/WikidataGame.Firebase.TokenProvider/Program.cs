using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace WikidataGame.Firebase.TokenProvider
{
    public class Program
    {
        private static readonly string Uid = "REeguvVom5bfEjwTfVlLiuyTmv33";
        private static readonly string FilePath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName, "q-wiki-57251382-firebase-adminsdk-8whmj-64e446ddda.json");
        private static readonly string FirebaseApiKey = "AIzaSyCHEZfGA1-3U70XF-xUqZb36XtEM1M2_jk"; //not security relevant
        static async Task Main(string[] args)
        {
            Console.WriteLine($"Requesting an id token for user {Uid}...");
            var idToken = await RequestIdTokenAsync(Uid);
            Console.WriteLine(idToken);
            Console.ReadLine();
        }

        public static async Task<string> RequestIdTokenAsync(string uid)
        {
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile(FilePath)
            });

            var customToken = await FirebaseAuth.DefaultInstance.CreateCustomTokenAsync(uid);

            var client = new HttpClient();
            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"https://identitytoolkit.googleapis.com/v1/accounts:signInWithCustomToken?key={FirebaseApiKey}"),
                Headers = {
                    { HttpRequestHeader.ContentType.ToString(), "application/json" }
                },
                Content = new StringContent(JsonConvert.SerializeObject( new { token = customToken, returnSecureToken = true }))
            };
            var response = await client.SendAsync(httpRequestMessage);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStringAsync();
                JObject creds = JObject.Parse(content);
                return creds["idToken"].ToString();
            }
            return $"Failed with status code {response.StatusCode}!";
        }
    }
}
