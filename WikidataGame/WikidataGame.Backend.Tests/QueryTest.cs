using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WikidataGame.Backend.Models;
using WikidataGame.Backend.Services;
using Xunit;
using Xunit.Abstractions;

namespace WikidataGame.Backend.Tests
{
    public class QueryTest
    {
        private readonly ITestOutputHelper _output;

        public QueryTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Theory]
        [ClassData(typeof(QueryTestData))]
        public void QueryWikidata_WithAllQuestions_ReturnsAnswerOptions(Question q)
        {
            _output.WriteLine($"Question Info: Id({q.Id}), Type({q.MiniGameType.ToString()}), Description({q.TaskDescription})");
            TestMinigameService service = new TestMinigameService(Db.Instance.MinigameRepo, Db.Instance.Context);

            var result = service.QueryWikidata(q.SparqlQuery);
            Assert.True(result.Count == 4);
            // result is tuple(q,a)
            Assert.False(string.IsNullOrWhiteSpace(result.First().Item1));
            foreach (var option in result)
            {
                Assert.False(string.IsNullOrWhiteSpace(option.Item2));
            }
        }

        [Theory]
        [ClassData(typeof(QueryTestData))]
        public void QueryWikidata_WithAllQuestions_ReturnsDifferentQuestions(Question q)
        {
            _output.WriteLine($"Question Info: Id({q.Id}), Type({q.MiniGameType.ToString()}), Description({q.TaskDescription})");
            TestMinigameService service = new TestMinigameService(Db.Instance.MinigameRepo, Db.Instance.Context);
            var result = service.QueryWikidata(q.SparqlQuery);

            TestMinigameService service2 = new TestMinigameService(Db.Instance.MinigameRepo, Db.Instance.Context);
            var result2 = service2.QueryWikidata(q.SparqlQuery);
            Assert.NotEqual(result, result2);
        }
    }
}
