using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WikidataGame.Backend.Dto;
using WikidataGame.Backend.Helpers;
using WikidataGame.Backend.Repos;
using WikidataGame.Backend.Services;

namespace WikidataGame.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : CustomControllerBase
    {
        private readonly AuthService _authService;
        private const string AnonPrefix = "anon";

        public AuthController(
            DataContext dataContext,
            UserManager<Models.User> userManager,
            IGameRepository gameRepo,
            INotificationService notificationService,
            AuthService authService) : base(dataContext, userManager, gameRepo, notificationService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Authenticates a player using username/password
        /// </summary>
        /// <param name="username">Username (min. 3 chars)</param>
        /// <param name="password">Password (min. 8 chars)</param>
        /// <param name="pushToken">push token generated through firebase/apns</param>
        /// <returns>Information on how to authenticate</returns>
        [HttpGet]
        [ProducesResponseType(typeof(AuthInfo), StatusCodes.Status200OK)]
        public async Task<ActionResult<AuthInfo>> Authenticate(
            [FromHeader(Name = "X-Username")] string username,
            [FromHeader(Name = "X-Password")] string password,
            [FromHeader(Name = "X-Push-Token")] string pushToken)
        {
            if(!username.StartsWith(AnonPrefix))
            {
                username = $"{AnonPrefix}-{username}";
            }
            try
            {
                var user = await _userManager.FindByNameAsync(username);
                if (user == null)
                {
                    //create
                    var userToCreate = new Models.User { UserName = username };
                    var idresult = await _userManager.CreateAsync(userToCreate, password);
                    if (!idresult.Succeeded)
                    {
                        throw new IdentityErrorException(idresult.Errors);
                    }
                    user = await _userManager.FindByIdAsync(userToCreate.Id.ToString());
                }
                else
                {
                    //login
                    if (!await _userManager.CheckPasswordAsync(user, password))
                        return Unauthorized();

                    var idresult = await _userManager.UpdateAsync(user);
                    if (!idresult.Succeeded)
                    {
                        throw new IdentityErrorException(idresult.Errors);
                    }
                }

                await RegisterPushForUserAsync(user, pushToken);
                var authInfo = JwtTokenHelper.CreateJwtToken(user);
                Response.Headers.Add("WWW-Authenticate", $"Bearer {authInfo.Bearer}");
                return Ok(authInfo);
            }
            catch (IdentityErrorException ex)
            {
                return BadRequest(ex.ToString());
            }

        }

        /// <summary>
        /// Authenticates a player using Google Play Services
        /// </summary>
        /// <param name="username">Username (min. 3 chars)</param>
        /// <param name="authCode">backend auth code</param>
        /// <param name="pushToken">push token generated through firebase/apns</param>
        /// <returns>Information on how to authenticate</returns>
        [HttpGet("Google")]
        [ProducesResponseType(typeof(AuthInfo), StatusCodes.Status200OK)]
        public async Task<ActionResult<AuthInfo>> AuthenticateGooglePlay(
            [FromHeader(Name = "X-Username")] string username,
            [FromHeader(Name = "X-Auth-Code")] string authCode,
            [FromHeader(Name = "X-Push-Token")] string pushToken)
        {
            if (string.IsNullOrWhiteSpace(authCode))
                return BadRequest(new { message = "Authentication token needs to be supplied" });

            var verificationResponse = await _authService.VerifyAuthCodeAsync(authCode);
            if (!verificationResponse.Success)
            {
                return Unauthorized();
            }

            try
            {
                var user = await CreateOrUpdateUserWithLoginProviderAsync(
                    "Google",
                    pushToken,
                    username,
                    verificationResponse.Response);

                var authInfo = JwtTokenHelper.CreateJwtToken(user);
                Response.Headers.Add("WWW-Authenticate", $"Bearer {authInfo.Bearer}");
                return Ok(authInfo);
            }
            catch (IdentityErrorException ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        private async Task<Models.User> CreateOrUpdateUserWithLoginProviderAsync(
            string provider,
            string pushToken,
            string username,
            GoogleResponse response)
        {
            var user = await _userManager.FindByLoginAsync(provider, response.GooglePlayId);
            if (user == null)
            {
                //create
                var userToCreate = new Models.User { UserName = username, ProfileImageUrl = response.GooglePlayProfileImage };
                var idresult = await _userManager.CreateAsync(userToCreate);
                if (!idresult.Succeeded)
                {
                    throw new IdentityErrorException(idresult.Errors);
                }
                user = await _userManager.FindByIdAsync(userToCreate.Id.ToString());
                idresult = await _userManager.AddLoginAsync(user, new UserLoginInfo(provider, response.GooglePlayId, string.Empty));
                if (!idresult.Succeeded)
                {
                    throw new IdentityErrorException(idresult.Errors);
                }
            }
            else
            {
                //update
                user.UserName = username;
                user.ProfileImageUrl = response.GooglePlayProfileImage;
                var idresult = await _userManager.UpdateAsync(user);
                if (!idresult.Succeeded)
                {
                    throw new IdentityErrorException(idresult.Errors);
                }
            }

            var tokens = new Dictionary<string, string> {
                        { "access", response.GooglePlayAccessToken }
                    };
            if (!string.IsNullOrWhiteSpace(response.GooglePlayRefreshToken))
            {
                tokens.Add("refresh", response.GooglePlayRefreshToken);
            }

            foreach (var additionalToken in tokens)
            {
                var idresult = await _userManager.SetAuthenticationTokenAsync(user, provider, additionalToken.Key, additionalToken.Value);
                if (!idresult.Succeeded)
                {
                    throw new IdentityErrorException(idresult.Errors);
                }
            }
            

            await RegisterPushForUserAsync(user, pushToken);
            return user;
        }

        private async Task RegisterPushForUserAsync(Models.User user, string pushToken)
        {
            if (string.IsNullOrWhiteSpace(pushToken))
            {
                if (!string.IsNullOrEmpty(user.PushRegistrationId)) //remove push registration
                {
                    await _notificationService.DeleteUserAsync(user);
                    user.PushRegistrationId = string.Empty;
                }
            }
            else
            {
                var registrationId = await _notificationService.RegisterOrUpdatePushChannelAsync(user, new DeviceRegistration
                {
                    Handle = pushToken,
                    Platform = "fcm",
                    Tags = new string[0]
                });
                user.PushRegistrationId = registrationId;
            }
            await _userManager.UpdateAsync(user);
        }
    }
}