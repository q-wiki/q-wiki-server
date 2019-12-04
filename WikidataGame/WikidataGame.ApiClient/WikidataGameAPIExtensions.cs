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
            public static Player PostFriend(this IWikidataGameAPI operations, System.Guid? friendId = default(System.Guid?))
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
            public static async Task<Player> PostFriendAsync(this IWikidataGameAPI operations, System.Guid? friendId = default(System.Guid?), CancellationToken cancellationToken = default(CancellationToken))
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
            public static Player DeleteFriend(this IWikidataGameAPI operations, System.Guid friendId)
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
            public static async Task<Player> DeleteFriendAsync(this IWikidataGameAPI operations, System.Guid friendId, CancellationToken cancellationToken = default(CancellationToken))
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
            /// Creates a new game and matches the player with an opponent
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='mapWidth'>
            /// Width of generated map
            /// </param>
            /// <param name='mapHeight'>
            /// Height of generated map
            /// </param>
            /// <param name='accessibleTilesCount'>
            /// How many accessible tiles the generated map should contain.
            /// </param>
            public static GameInfo CreateNewGame(this IWikidataGameAPI operations, int? mapWidth = 10, int? mapHeight = 10, int? accessibleTilesCount = 70)
            {
                return operations.CreateNewGameAsync(mapWidth, mapHeight, accessibleTilesCount).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Creates a new game and matches the player with an opponent
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='mapWidth'>
            /// Width of generated map
            /// </param>
            /// <param name='mapHeight'>
            /// Height of generated map
            /// </param>
            /// <param name='accessibleTilesCount'>
            /// How many accessible tiles the generated map should contain.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<GameInfo> CreateNewGameAsync(this IWikidataGameAPI operations, int? mapWidth = 10, int? mapHeight = 10, int? accessibleTilesCount = 70, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.CreateNewGameWithHttpMessagesAsync(mapWidth, mapHeight, accessibleTilesCount, null, cancellationToken).ConfigureAwait(false))
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
            public static Game RetrieveGameState(this IWikidataGameAPI operations, System.Guid gameId)
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
            public static async Task<Game> RetrieveGameStateAsync(this IWikidataGameAPI operations, System.Guid gameId, CancellationToken cancellationToken = default(CancellationToken))
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
            public static void DeleteGame(this IWikidataGameAPI operations, System.Guid gameId)
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
            public static async Task DeleteGameAsync(this IWikidataGameAPI operations, System.Guid gameId, CancellationToken cancellationToken = default(CancellationToken))
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
            public static MiniGame InitalizeMinigame(this IWikidataGameAPI operations, System.Guid gameId, MiniGameInit minigameParams = default(MiniGameInit))
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
            public static async Task<MiniGame> InitalizeMinigameAsync(this IWikidataGameAPI operations, System.Guid gameId, MiniGameInit minigameParams = default(MiniGameInit), CancellationToken cancellationToken = default(CancellationToken))
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
            public static MiniGame RetrieveMinigameInfo(this IWikidataGameAPI operations, System.Guid gameId, System.Guid minigameId)
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
            public static async Task<MiniGame> RetrieveMinigameInfoAsync(this IWikidataGameAPI operations, System.Guid gameId, System.Guid minigameId, CancellationToken cancellationToken = default(CancellationToken))
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
            public static MiniGameResult AnswerMinigame(this IWikidataGameAPI operations, System.Guid gameId, System.Guid minigameId, IList<string> answers = default(IList<string>))
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
            public static async Task<MiniGameResult> AnswerMinigameAsync(this IWikidataGameAPI operations, System.Guid gameId, System.Guid minigameId, IList<string> answers = default(IList<string>), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.AnswerMinigameWithHttpMessagesAsync(gameId, minigameId, answers, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

    }
}
