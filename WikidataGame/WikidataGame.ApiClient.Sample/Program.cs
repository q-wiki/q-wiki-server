using Microsoft.Rest;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WikidataGame.ApiClient.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            RunApiRequests().Wait();
        }

        static async Task RunApiRequests()
        {
            //Authentication
            var apiClient = new WikidataGameAPI(new Uri("https://wikidatagame.azurewebsites.net"), new TokenCredentials("auth"));

            CancellationTokenSource cts = new CancellationTokenSource(); // <-- Cancellation Token if you want to cancel the request, user quits, etc. [cts.Cancel()]
            var auth = await apiClient.AuthenticateAsync("123", "test", cts.Token);
            Console.WriteLine($"Bearer {auth.Bearer}");

            //Create a new api client with the obtained bearer token for all other (authorized) requests
            var apiClient2 = new WikidataGameAPI(new Uri("https://wikidatagame.azurewebsites.net"), new TokenCredentials(auth.Bearer));

            CancellationTokenSource cts2 = new CancellationTokenSource();
            var game = await apiClient2.CreateNewGameAsync(cts2.Token);
            Console.WriteLine($"Started game {game.GameId}.");

            CancellationTokenSource cts3 = new CancellationTokenSource();
            var fullGame = await apiClient2.RetrieveGameStateAsync(game.GameId, cts3.Token);
            Console.WriteLine($"My player id is {fullGame.Me.Id}.");
            Console.ReadLine();
        }
    }
}