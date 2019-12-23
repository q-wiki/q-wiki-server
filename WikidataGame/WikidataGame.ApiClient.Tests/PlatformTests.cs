using Microsoft.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace WikidataGame.ApiClient.Tests
{
    public class PlatformTests : ClientTestBase
    {
        [Fact]
        public async void RequestStats_ForPlatform_ReturnsStats()
        {
            var apiClient = new WikidataGameAPI(new Uri(BaseUrl), new TokenCredentials("no-auth"));
            var stats = await apiClient.GetPlatformStatsAsync();

            ModelAssertion.AssertStats(stats);
        }

        [Fact]
        public async void RequestCategories_ForPlatform_ReturnsCategories()
        {
            var apiClient = new WikidataGameAPI(new Uri(BaseUrl), new TokenCredentials("no-auth"));
            var categories = await apiClient.GetPlatformCategoriesAsync();

            Assert.NotEmpty(categories);
            Assert.InRange(categories.Count, 3, int.MaxValue); // min 3 categories
            Assert.All(categories, c => ModelAssertion.AssertCategory(c));
        }

        [Fact]
        public async void RequestQuestions_ForPlatform_ReturnsQuestions()
        {
            var apiClient = new WikidataGameAPI(new Uri(BaseUrl), new TokenCredentials("no-auth"));
            var questions = await apiClient.GetPlatformQuestionsAsync();

            Assert.NotEmpty(questions);
            Assert.InRange(questions.Count, 10, int.MaxValue); // min 10 questions

            Assert.All(questions, q => ModelAssertion.AssertQuestion(q));
        }

        [Fact]
        public async void RequestMinigameDetails_ForPlatform_ReturnsDetailedMinigame()
        {
            var authInfo = await RetrieveBearerAsync();
            var apiClient = new WikidataGameAPI(new Uri(BaseUrl), new TokenCredentials(authInfo.Bearer));
            var gameInfo = await apiClient.CreateNewGameAsync();
            var game = await apiClient.RetrieveGameStateAsync(gameInfo.GameId);
            if (game.AwaitingOpponentToJoin.Value)
            {
                var authInfo2 = await RetrieveBearerAsync();
                apiClient = new WikidataGameAPI(new Uri(BaseUrl), new TokenCredentials(authInfo2.Bearer));
                var gameInfo2 = await apiClient.CreateNewGameAsync();
                game = await apiClient.RetrieveGameStateAsync(gameInfo2.GameId);
            }

            var tile = game.Tiles.Where(cg => cg.Count(c => c != null) > 0).First().Where(c => c != null).First();
            var (Result, Id) = await CreateAndAnswerMinigameRandomly(apiClient, game.Id, tile);

            var minigameDetails = await apiClient.GetPlatformMinigameByIdAsync(Id);
            ModelAssertion.AssertDetailedMinigame(minigameDetails);
        }

        [Fact]
        public async void AddRating_ForQuestion_ReturnsUpdatedRating()
        {
            var apiClient = new WikidataGameAPI(new Uri(BaseUrl), new TokenCredentials("no-auth"));
            var questions = await apiClient.GetPlatformQuestionsAsync();
            var random = new Random();
            var question = questions[random.Next(0, questions.Count - 1)];
            var rating = random.Next(1, 5);
            var updatedQuestion = await apiClient.AddPlatformQuestionRatingAsync(question.Id, rating);

            ModelAssertion.AssertQuestion(updatedQuestion);
            if(rating == question.Rating || question.Rating == 0)
            {
                Assert.Equal(rating, updatedQuestion.Rating);
            }
            else
            {
                Assert.NotEqual(rating, updatedQuestion.Rating);
            }
        }

        [Fact]
        public async void AddQuestion_FromFixedValues_ReturnsCreatedQuestion()
        {
            var apiClient = new WikidataGameAPI(new Uri(BaseUrl), new TokenCredentials("no-auth"));
            var categories = await apiClient.GetPlatformCategoriesAsync();
            var question = new Models.Question
            {
                Category = new Models.Category
                {
                    Id = categories.First().Id
                },
                MiniGameType = 0,
                SparqlQuery = "This is a test query",
                TaskDescription = "What is {0}?"
            };
            var createdQuestion = await apiClient.AddPlatformQuestionAsync(question);

            ModelAssertion.AssertQuestion(createdQuestion);
            Assert.Equal(question.SparqlQuery, createdQuestion.SparqlQuery);
            Assert.Equal(question.MiniGameType, createdQuestion.MiniGameType);
            Assert.Equal(question.TaskDescription, createdQuestion.TaskDescription);
            Assert.Equal(question.Category.Id, createdQuestion.Category.Id);
            Assert.Equal(0, createdQuestion.Rating);
            Assert.Equal(0, createdQuestion.Status); //pending
        }
    }
}
