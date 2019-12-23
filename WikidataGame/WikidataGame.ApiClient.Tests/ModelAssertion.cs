using System;
using System.Collections.Generic;
using System.Text;
using WikidataGame.Models;
using Xunit;

namespace WikidataGame.ApiClient.Tests
{
    public static class ModelAssertion
    {
        public static void AssertCategory(Category category)
        {
            Assert.NotNull(category);
            AssertGuid(category.Id);
            Assert.False(string.IsNullOrEmpty(category.Title));
        }

        public static void AssertQuestion(Question question)
        {
            Assert.NotNull(question);
            AssertGuid(question.Id);
            Assert.False(string.IsNullOrEmpty(question.SparqlQuery));
            Assert.False(string.IsNullOrEmpty(question.TaskDescription));
            AssertCategory(question.Category);
            Assert.True(question.Rating.HasValue);
            Assert.InRange(question.Rating.Value, 0, 5);
            Assert.True(question.Status.HasValue);
        }

        public static void AssertAuthInfo(AuthInfo authInfo)
        {
            Assert.NotNull(authInfo);
            Assert.False(string.IsNullOrWhiteSpace(authInfo.Bearer));
            AssertPlayer(authInfo.User);
        }

        public static void AssertPlayer(Player player)
        {
            Assert.NotNull(player);
            AssertGuid(player.Id);
            Assert.False(string.IsNullOrEmpty(player.Name));
        }

        public static void AssertGameRequest(GameRequest gr)
        {
            Assert.NotNull(gr);
            AssertGuid(gr.Id);
            AssertPlayer(gr.Recipient);
            AssertPlayer(gr.Sender);
        }

        public static void AssertGameInfo(GameInfo gi)
        {
            Assert.NotNull(gi);
            AssertGuid(gi.GameId);
            Assert.True(gi.IsAwaitingOpponentToJoin.HasValue);
        }

        public static void AssertGame(Game game)
        {
            Assert.NotNull(game);
            AssertGuid(game.Id);
            Assert.True(game.AwaitingOpponentToJoin.HasValue);
            AssertPlayer(game.Me);
            if (!game.AwaitingOpponentToJoin.Value)
            {
                AssertPlayer(game.Opponent);
            }
            Assert.NotEmpty(game.Tiles);
            Assert.All(game.Tiles, tilelist =>
            {
                Assert.All(tilelist, tile => AssertTile(tile));
            });
        }

        public static void AssertTile(Tile tile)
        {
            if (tile != null)
            {
                AssertGuid(tile.Id);
                Assert.NotNull(tile.Difficulty);
                Assert.NotEmpty(tile.AvailableCategories);
            }
        }

        public static void AssertMinigame(MiniGame minigame)
        {
            Assert.NotNull(minigame);
            AssertGuid(minigame.Id);
            Assert.InRange(minigame.AnswerOptions.Count, 4, 4);
            Assert.All(minigame.AnswerOptions, option => Assert.False(string.IsNullOrEmpty(option)));
            Assert.False(string.IsNullOrEmpty(minigame.TaskDescription));
            Assert.NotNull(minigame.Type);
        }

        public static void AssertMinigameResult(MiniGameResult mgr)
        {
            Assert.NotNull(mgr);
            Assert.NotEmpty(mgr.CorrectAnswer);
            Assert.All(mgr.CorrectAnswer, a => Assert.False(string.IsNullOrEmpty(a)));
            Assert.True(mgr.IsWin.HasValue);
            AssertGuid(mgr.NextMovePlayerId);
            Assert.All(mgr.Tiles, tilelist =>
            {
                Assert.NotEmpty(tilelist);
                Assert.All(tilelist, tile => AssertTile(tile));
            });
        }

        public static void AssertStats(PlatformStats stats)
        {
            Assert.True(stats.NumberOfCategories.HasValue);
            Assert.InRange(stats.NumberOfCategories.Value, 3, int.MaxValue); // min 3 categories

            Assert.True(stats.NumberOfQuestions.HasValue);
            Assert.InRange(stats.NumberOfQuestions.Value, stats.NumberOfCategories.Value, int.MaxValue); // min 1 question per category

            Assert.True(stats.NumberOfGamesPlayed.HasValue);

            Assert.True(stats.NumberOfContributions.HasValue);
        }

        public static void AssertDetailedMinigame(DetailedMiniGame dmg)
        {
            Assert.NotNull(dmg);
            AssertGuid(dmg.Id);
            Assert.InRange(dmg.AnswerOptions.Count, 4, 4);
            Assert.All(dmg.AnswerOptions, option => Assert.False(string.IsNullOrEmpty(option)));
            Assert.False(string.IsNullOrEmpty(dmg.TaskDescription));
            Assert.NotNull(dmg.Type);

            Assert.NotEmpty(dmg.CorrectAnswer);
            Assert.All(dmg.CorrectAnswer, a => Assert.False(string.IsNullOrEmpty(a)));
            AssertQuestion(dmg.Question);
        }

        private static void AssertGuid(string id)
        {
            Assert.False(string.IsNullOrEmpty(id));
            Assert.True(Guid.TryParse(id, out var guid));
            Assert.False(guid == default);
        }
    }
}
