using Microsoft.Rest;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WikidataGame.Models;
using Xunit;

[assembly: CollectionBehavior(CollectionBehavior.CollectionPerClass, DisableTestParallelization = true)]
namespace WikidataGame.ApiClient.Tests
{
    public abstract class ClientTestBase
    {
        public const string BaseUrl = "http://localhost:57635/"; //"https://wikidatagame.azurewebsites.net";


        protected async Task<AuthInfo> RetrieveBearerAsync()
        {
            var apiClient = new WikidataGameAPI(new Uri(BaseUrl), new TokenCredentials("auth"));
            return await apiClient.AuthenticateAsync(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), string.Empty);
        }
    }
}
