using Microsoft.Azure.WebJobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;
using WikidataGame.Backend.Helpers;
using WikidataGame.Backend.Repos;
using WikidataGame.Backend.Services;
using Microsoft.Extensions.Configuration;

namespace WikidataGame.Backend.WebJob
{
    // To learn more about Microsoft Azure WebJobs SDK, please see https://go.microsoft.com/fwlink/?LinkID=320976
    internal class Program
    {
        public static IConfiguration Configuration { get; private set; }
        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        private static async Task Main()
        {
            var builder = new HostBuilder()
              .ConfigureWebJobs(b =>
              {
                  b.AddAzureStorageCoreServices().AddAzureStorage().AddTimers();
              })
              .ConfigureAppConfiguration(b =>
              {
              })
              .ConfigureLogging((context, b) =>
              {
                  b.AddConsole();
              });

            builder.ConfigureServices((services) => {
                Configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddEnvironmentVariables()
                    .Build();

                if (!string.IsNullOrWhiteSpace(Configuration.GetConnectionString("SQL")))
                {
                    services.AddDbContext<DataContext>(x => x.UseSqlServer(Configuration.GetConnectionString("SQL"))
                        .UseLazyLoadingProxies());
                }
                else
                {
#if DEBUG
                    var filename = Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\..\\WikidataGame.Backend", "qwiki.db");
#else
                    var filename = Path.Combine("d:\\home\\site\\wwwroot", "qwiki.db");
#endif
                    services.AddDbContext<DataContext>(x => x.UseSqlite($"Filename={filename}").UseLazyLoadingProxies());
                }
                services.AddSingleton<INotificationService>(new NotificationService(Configuration.GetConnectionString("NotificationHub")));
                services.AddScoped<IGameRepository, GameRepository>();
            });

            var host = builder.Build();
            using (host)
            {
                await host.RunAsync();
            }
        }
    }
}
