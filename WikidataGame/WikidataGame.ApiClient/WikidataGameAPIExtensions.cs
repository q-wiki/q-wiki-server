// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace WikidataGame
{
    using Models;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Extension methods for WikidataGameAPI.
    /// </summary>
    public static partial class WikidataGameAPIExtensions
    {
            /// <summary>
            /// Authenticates a player using username/password
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='xUsername'>
            /// Username (min. 3 chars)
            /// </param>
            /// <param name='xPassword'>
            /// Password (min. 8 chars)
            /// </param>
            /// <param name='xPushToken'>
            /// push token generated through firebase/apns
            /// </param>
            public static AuthInfo Authenticate(this IWikidataGameAPI operations, string xUsername = default(string), string xPassword = default(string), string xPushToken = default(string))
            {
                return operations.AuthenticateAsync(xUsername, xPassword, xPushToken).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Authenticates a player using username/password
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='xUsername'>
            /// Username (min. 3 chars)
            /// </param>
            /// <param name='xPassword'>
            /// Password (min. 8 chars)
            /// </param>
            /// <param name='xPushToken'>
            /// push token generated through firebase/apns
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<AuthInfo> AuthenticateAsync(this IWikidataGameAPI operations, string xUsername = default(string), string xPassword = default(string), string xPushToken = default(string), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.AuthenticateWithHttpMessagesAsync(xUsername, xPassword, xPushToken, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Authenticates a player using Google Play Services
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='xUsername'>
            /// Username (min. 3 chars)
            /// </param>
            /// <param name='xAuthCode'>
            /// backend auth code
            /// </param>
            /// <param name='xPushToken'>
            /// push token generated through firebase/apns
            /// </param>
            public static AuthInfo AuthenticateGooglePlay(this IWikidataGameAPI operations, string xUsername = default(string), string xAuthCode = default(string), string xPushToken = default(string))
            {
                return operations.AuthenticateGooglePlayAsync(xUsername, xAuthCode, xPushToken).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Authenticates a player using Google Play Services
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='xUsername'>
            /// Username (min. 3 chars)
            /// </param>
            /// <param name='xAuthCode'>
            /// backend auth code
            /// </param>
            /// <param name='xPushToken'>
            /// push token generated through firebase/apns
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<AuthInfo> AuthenticateGooglePlayAsync(this IWikidataGameAPI operations, string xUsername = default(string), string xAuthCode = default(string), string xPushToken = default(string), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.AuthenticateGooglePlayWithHttpMessagesAsync(xUsername, xAuthCode, xPushToken, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Retrieves the friendlist for the signed in user
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            public static IList<Player> GetFriends(this IWikidataGameAPI operations)
            {
                return operations.GetFriendsAsync().GetAwaiter().GetResult();
            }

            /// <summary>
            /// Retrieves the friendlist for the signed in user
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<IList<Player>> GetFriendsAsync(this IWikidataGameAPI operations, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetFriendsWithHttpMessagesAsync(null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Adds a user as a friend
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='friendId'>
            /// The user id of the player that should be added
            /// </param>
            public static Player PostFriend(this IWikidataGameAPI operations, string friendId = default(string))
            {
                return operations.PostFriendAsync(friendId).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Adds a user as a friend
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='friendId'>
            /// The user id of the player that should be added
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<Player> PostFriendAsync(this IWikidataGameAPI operations, string friendId = default(string), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.PostFriendWithHttpMessagesAsync(friendId, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Removes a friend from the friend list
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='friendId'>
            /// User id of the friend to be removed
            /// </param>
            public static Player DeleteFriend(this IWikidataGameAPI operations, string friendId)
            {
                return operations.DeleteFriendAsync(friendId).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Removes a friend from the friend list
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='friendId'>
            /// User id of the friend to be removed
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<Player> DeleteFriendAsync(this IWikidataGameAPI operations, string friendId, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.DeleteFriendWithHttpMessagesAsync(friendId, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Retrieves users (limit 10) with a username similar to the query string
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='query'>
            /// Username or part of a username (min. 3 characters)
            /// </param>
            public static IList<Player> GetFindFriends(this IWikidataGameAPI operations, string query = default(string))
            {
                return operations.GetFindFriendsAsync(query).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Retrieves users (limit 10) with a username similar to the query string
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='query'>
            /// Username or part of a username (min. 3 characters)
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<IList<Player>> GetFindFriendsAsync(this IWikidataGameAPI operations, string query = default(string), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetFindFriendsWithHttpMessagesAsync(query, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Retrieves all game requests for the authenticated player
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            public static GameRequestList GetGameRequests(this IWikidataGameAPI operations)
            {
                return operations.GetGameRequestsAsync().GetAwaiter().GetResult();
            }

            /// <summary>
            /// Retrieves all game requests for the authenticated player
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<GameRequestList> GetGameRequestsAsync(this IWikidataGameAPI operations, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetGameRequestsWithHttpMessagesAsync(null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Sends a game request to the specified user
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='userId'>
            /// user id
            /// </param>
            public static GameRequest RequestMatch(this IWikidataGameAPI operations, string userId = default(string))
            {
                return operations.RequestMatchAsync(userId).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Sends a game request to the specified user
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='userId'>
            /// user id
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<GameRequest> RequestMatchAsync(this IWikidataGameAPI operations, string userId = default(string), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.RequestMatchWithHttpMessagesAsync(userId, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Deletes a game request for the sender or recipient
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='gameRequestId'>
            /// game request identifier
            /// </param>
            public static void DeleteGameRequest(this IWikidataGameAPI operations, string gameRequestId = default(string))
            {
                operations.DeleteGameRequestAsync(gameRequestId).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Deletes a game request for the sender or recipient
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='gameRequestId'>
            /// game request identifier
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task DeleteGameRequestAsync(this IWikidataGameAPI operations, string gameRequestId = default(string), CancellationToken cancellationToken = default(CancellationToken))
            {
                (await operations.DeleteGameRequestWithHttpMessagesAsync(gameRequestId, null, cancellationToken).ConfigureAwait(false)).Dispose();
            }

            /// <summary>
            /// Creates a new game by accepting a game request
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='gameRequestId'>
            /// game request identifier
            /// </param>
            public static GameInfo CreateNewGameByRequest(this IWikidataGameAPI operations, string gameRequestId = default(string))
            {
                return operations.CreateNewGameByRequestAsync(gameRequestId).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Creates a new game by accepting a game request
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='gameRequestId'>
            /// game request identifier
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<GameInfo> CreateNewGameByRequestAsync(this IWikidataGameAPI operations, string gameRequestId = default(string), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.CreateNewGameByRequestWithHttpMessagesAsync(gameRequestId, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Retrieves all currently running games for the authenticated player
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            public static IList<GameInfo> GetGames(this IWikidataGameAPI operations)
            {
                return operations.GetGamesAsync().GetAwaiter().GetResult();
            }

            /// <summary>
            /// Retrieves all currently running games for the authenticated player
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<IList<GameInfo>> GetGamesAsync(this IWikidataGameAPI operations, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetGamesWithHttpMessagesAsync(null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Creates a new game and matches the player with an opponent
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            public static GameInfo CreateNewGame(this IWikidataGameAPI operations)
            {
                return operations.CreateNewGameAsync().GetAwaiter().GetResult();
            }

            /// <summary>
            /// Creates a new game and matches the player with an opponent
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<GameInfo> CreateNewGameAsync(this IWikidataGameAPI operations, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.CreateNewGameWithHttpMessagesAsync(null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Retrieves information on the specified game
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='gameId'>
            /// game identifier
            /// </param>
            public static Game RetrieveGameState(this IWikidataGameAPI operations, string gameId)
            {
                return operations.RetrieveGameStateAsync(gameId).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Retrieves information on the specified game
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='gameId'>
            /// game identifier
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<Game> RetrieveGameStateAsync(this IWikidataGameAPI operations, string gameId, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.RetrieveGameStateWithHttpMessagesAsync(gameId, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Stops/deletes the specified game
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='gameId'>
            /// game identifier
            /// </param>
            public static void DeleteGame(this IWikidataGameAPI operations, string gameId)
            {
                operations.DeleteGameAsync(gameId).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Stops/deletes the specified game
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='gameId'>
            /// game identifier
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task DeleteGameAsync(this IWikidataGameAPI operations, string gameId, CancellationToken cancellationToken = default(CancellationToken))
            {
                (await operations.DeleteGameWithHttpMessagesAsync(gameId, null, cancellationToken).ConfigureAwait(false)).Dispose();
            }

            /// <summary>
            /// Initializes a new minigame
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='gameId'>
            /// game identifier
            /// </param>
            /// <param name='minigameParams'>
            /// minigame information containing category and tile identifier
            /// </param>
            public static MiniGame InitalizeMinigame(this IWikidataGameAPI operations, string gameId, MiniGameInit minigameParams = default(MiniGameInit))
            {
                return operations.InitalizeMinigameAsync(gameId, minigameParams).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Initializes a new minigame
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='gameId'>
            /// game identifier
            /// </param>
            /// <param name='minigameParams'>
            /// minigame information containing category and tile identifier
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<MiniGame> InitalizeMinigameAsync(this IWikidataGameAPI operations, string gameId, MiniGameInit minigameParams = default(MiniGameInit), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.InitalizeMinigameWithHttpMessagesAsync(gameId, minigameParams, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Retrieves the details of the specified minigame
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='gameId'>
            /// game identifier
            /// </param>
            /// <param name='minigameId'>
            /// minigame identifier
            /// </param>
            public static MiniGame RetrieveMinigameInfo(this IWikidataGameAPI operations, string gameId, string minigameId)
            {
                return operations.RetrieveMinigameInfoAsync(gameId, minigameId).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Retrieves the details of the specified minigame
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='gameId'>
            /// game identifier
            /// </param>
            /// <param name='minigameId'>
            /// minigame identifier
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<MiniGame> RetrieveMinigameInfoAsync(this IWikidataGameAPI operations, string gameId, string minigameId, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.RetrieveMinigameInfoWithHttpMessagesAsync(gameId, minigameId, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Post the answer(s) to the specified minigame
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='gameId'>
            /// game identifier
            /// </param>
            /// <param name='minigameId'>
            /// minigame identifier
            /// </param>
            /// <param name='answers'>
            /// answer(s)
            /// </param>
            public static MiniGameResult AnswerMinigame(this IWikidataGameAPI operations, string gameId, string minigameId, IList<string> answers = default(IList<string>))
            {
                return operations.AnswerMinigameAsync(gameId, minigameId, answers).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Post the answer(s) to the specified minigame
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='gameId'>
            /// game identifier
            /// </param>
            /// <param name='minigameId'>
            /// minigame identifier
            /// </param>
            /// <param name='answers'>
            /// answer(s)
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<MiniGameResult> AnswerMinigameAsync(this IWikidataGameAPI operations, string gameId, string minigameId, IList<string> answers = default(IList<string>), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.AnswerMinigameWithHttpMessagesAsync(gameId, minigameId, answers, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Retrieves statistics containing the number of categories, games played and
            /// questions added
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            public static PlatformStats GetPlatformStats(this IWikidataGameAPI operations)
            {
                return operations.GetPlatformStatsAsync().GetAwaiter().GetResult();
            }

            /// <summary>
            /// Retrieves statistics containing the number of categories, games played and
            /// questions added
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<PlatformStats> GetPlatformStatsAsync(this IWikidataGameAPI operations, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetPlatformStatsWithHttpMessagesAsync(null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Retrieves detailed information on a bygone minigame by id
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='minigameId'>
            /// minigame identifier
            /// </param>
            public static DetailedMiniGame GetPlatformMinigameById(this IWikidataGameAPI operations, string minigameId)
            {
                return operations.GetPlatformMinigameByIdAsync(minigameId).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Retrieves detailed information on a bygone minigame by id
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='minigameId'>
            /// minigame identifier
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<DetailedMiniGame> GetPlatformMinigameByIdAsync(this IWikidataGameAPI operations, string minigameId, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetPlatformMinigameByIdWithHttpMessagesAsync(minigameId, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Retrieves a list of all available questions
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            public static IList<Question> GetPlatformQuestions(this IWikidataGameAPI operations)
            {
                return operations.GetPlatformQuestionsAsync().GetAwaiter().GetResult();
            }

            /// <summary>
            /// Retrieves a list of all available questions
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<IList<Question>> GetPlatformQuestionsAsync(this IWikidataGameAPI operations, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetPlatformQuestionsWithHttpMessagesAsync(null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='question'>
            /// </param>
            public static Question AddPlatformQuestion(this IWikidataGameAPI operations, Question question = default(Question))
            {
                return operations.AddPlatformQuestionAsync(question).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='question'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<Question> AddPlatformQuestionAsync(this IWikidataGameAPI operations, Question question = default(Question), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.AddPlatformQuestionWithHttpMessagesAsync(question, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Adds a rating for the specified question
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='questionId'>
            /// question identifier
            /// </param>
            /// <param name='rating'>
            /// rating (1-5)
            /// </param>
            public static Question AddPlatformQuestionRating(this IWikidataGameAPI operations, string questionId, int rating)
            {
                return operations.AddPlatformQuestionRatingAsync(questionId, rating).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Adds a rating for the specified question
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='questionId'>
            /// question identifier
            /// </param>
            /// <param name='rating'>
            /// rating (1-5)
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<Question> AddPlatformQuestionRatingAsync(this IWikidataGameAPI operations, string questionId, int rating, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.AddPlatformQuestionRatingWithHttpMessagesAsync(questionId, rating, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Retrieves a list of all available categories
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            public static IList<Category> GetPlatformCategories(this IWikidataGameAPI operations)
            {
                return operations.GetPlatformCategoriesAsync().GetAwaiter().GetResult();
            }

            /// <summary>
            /// Retrieves a list of all available categories
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<IList<Category>> GetPlatformCategoriesAsync(this IWikidataGameAPI operations, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetPlatformCategoriesWithHttpMessagesAsync(null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

    }
}
