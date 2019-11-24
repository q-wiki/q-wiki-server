using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace WikidataGame.Firebase.TokenProvider
{
    public class Program
    {
        private static readonly string ProjectFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private static readonly string ConfigFilePath = Path.Combine(ProjectFolder, "config.json");
        private static readonly string FirebaseFilePath = Path.Combine(ProjectFolder, "q-wiki-57251382-firebase-adminsdk-8whmj-64e446ddda.json");

        static async Task Main(string[] args)
        {
            Console.WriteLine($"Requesting an id token...");
            var idToken = await RequestIdTokenAsync();
            Console.WriteLine(idToken);
            Console.ReadLine();
        }

        public static (string, string) RetrieveConfig()
        {
            using (StreamReader r = new StreamReader(ConfigFilePath))
            {
                string json = r.ReadToEnd();
                JObject config = JObject.Parse(json);
                return (config["uid"].ToString(), config["firebase_api_key"].ToString());
            }
        }

        public static async Task<string> RequestIdTokenAsync()
        {
            var config = RetrieveConfig();
            return await RequestIdTokenAsync(config.Item1, config.Item2);
        }

        public static async Task<string> RequestIdTokenAsync(string uid, string firebaseApiKey)
        {
            if (FirebaseAuth.DefaultInstance == null)
            {
                FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromFile(FirebaseFilePath)
                });
            }

            var customToken = await FirebaseAuth.DefaultInstance.CreateCustomTokenAsync(uid);

            using (var client = new HttpClient())
            {
                var httpRequestMessage = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri($"https://identitytoolkit.googleapis.com/v1/accounts:signInWithCustomToken?key={firebaseApiKey}"),
                    Headers = {
                    { HttpRequestHeader.ContentType.ToString(), "application/json" }
                },
                    Content = new StringContent(JsonConvert.SerializeObject(new { token = customToken, returnSecureToken = true }))
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
}
