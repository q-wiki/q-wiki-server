using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WikidataGame.Backend.Dto;
using WikidataGame.Backend.Helpers;
using WikidataGame.Backend.Services;

namespace WikidataGame.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public const string AnonPrefix = "anon";

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
            [FromHeader(Name = "X-Push-Token")] string pushToken,
#pragma warning disable CS1573 // no xml comments for service injection
            [FromServices] UserManager<Models.User> userManager,
            [FromServices] IMapper mapper,
            [FromServices] INotificationService notificationService)
#pragma warning restore CS1573
        {
            if (string.IsNullOrEmpty(username))
            {
                return BadRequest();
            }

            if (!username.StartsWith(AnonPrefix))
            {
                username = $"{AnonPrefix}-{username}";
            }
            try
            {
                var user = await userManager.FindByNameAsync(username);
                if (user == null)
                {
                    //create
                    var userToCreate = new Models.User { UserName = username };
                    var idresult = await userManager.CreateAsync(userToCreate, password);
                    if (!idresult.Succeeded)
                    {
                        throw new IdentityErrorException(idresult.Errors);
                    }
                    user = await userManager.FindByIdAsync(userToCreate.Id.ToString());
                }
                else
                {
                    //login
                    if (!await userManager.CheckPasswordAsync(user, password))
                        return Unauthorized();

                    var idresult = await userManager.UpdateAsync(user);
                    if (!idresult.Succeeded)
                    {
                        throw new IdentityErrorException(idresult.Errors);
                    }
                }

                await RegisterPushForUserAsync(user, pushToken, userManager, notificationService);
                var authInfo = JwtTokenHelper.CreateJwtToken(user, mapper);
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
            [FromHeader(Name = "X-Push-Token")] string pushToken,
#pragma warning disable CS1573 // no xml comments for service injection
            [FromServices] AuthService authService,
            [FromServices] UserManager<Models.User> userManager,
            [FromServices] INotificationService notificationService,
            [FromServices] IMapper mapper)
#pragma warning restore CS1573
        {
            if (string.IsNullOrWhiteSpace(authCode))
                return BadRequest(new { message = "Authentication token needs to be supplied" });

            var verificationResponse = await authService.VerifyAuthCodeAsync(authCode);
            if (!verificationResponse.Success)
            {
                return Unauthorized();
            }

            try
            {
                var user = await CreateOrUpdateUserWithLoginProviderAsync(
                    "Google",
                    username,
                    verificationResponse.Response,
                    userManager);

                await RegisterPushForUserAsync(user, pushToken, userManager, notificationService);

                var authInfo = JwtTokenHelper.CreateJwtToken(user, mapper);
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
            string username,
            GoogleResponse response,
            UserManager<Models.User> userManager)
        {
            var user = await userManager.FindByLoginAsync(provider, response.GooglePlayId);
            if (user == null)
            {
                //create
                var userToCreate = new Models.User { UserName = username, ProfileImageUrl = response.GooglePlayProfileImage };
                var idresult = await userManager.CreateAsync(userToCreate);
                if (!idresult.Succeeded)
                {
                    throw new IdentityErrorException(idresult.Errors);
                }
                user = await userManager.FindByIdAsync(userToCreate.Id.ToString());
                idresult = await userManager.AddLoginAsync(user, new UserLoginInfo(provider, response.GooglePlayId, string.Empty));
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
                var idresult = await userManager.UpdateAsync(user);
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
                var idresult = await userManager.SetAuthenticationTokenAsync(user, provider, additionalToken.Key, additionalToken.Value);
                if (!idresult.Succeeded)
                {
                    throw new IdentityErrorException(idresult.Errors);
                }
            }
            
            return user;
        }

        private async Task RegisterPushForUserAsync(Models.User user, string pushToken, UserManager<Models.User> userManager, INotificationService notificationService)
        {
            if (string.IsNullOrWhiteSpace(pushToken))
            {
                if (!string.IsNullOrEmpty(user.PushRegistrationId)) //remove push registration
                {
                    await notificationService.DeleteUserAsync(user);
                    user.PushRegistrationId = string.Empty;
                }
            }
            else
            {
                var registrationId = await notificationService.RegisterOrUpdatePushChannelAsync(user, new DeviceRegistration
                {
                    Handle = pushToken,
                    Platform = "fcm",
                    Tags = new string[0]
                });
                user.PushRegistrationId = registrationId;
            }
            await userManager.UpdateAsync(user);
        }
    }
}