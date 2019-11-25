using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WikidataGame.Backend.Dto;
using WikidataGame.Backend.Helpers;
using WikidataGame.Backend.Repos;
using WikidataGame.Backend.Services;
using static WikidataGame.Backend.Repos.UserRepository;

namespace WikidataGame.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : CustomControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(
            DataContext dataContext,
            IUserRepository userRepo,
            IGameRepository gameRepo,
            IRepository<Models.Category, string> categoryRepo,
            INotificationService notificationService,
            AuthService authService) : base(dataContext, userRepo, gameRepo, categoryRepo, notificationService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Authenticates a player 
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="authToken">firebase authentication token</param>
        /// <param name="pushToken">push token generated through firebase/apns</param>
        /// <returns>Information on how to authenticate</returns>
        [HttpGet]
        [ProducesResponseType(typeof(AuthInfo), StatusCodes.Status200OK)]
        public async Task<ActionResult<AuthInfo>> Authenticate(
            [FromHeader(Name = "X-Username")] string username,
            [FromHeader(Name = "X-Auth-Token")] string authToken,
            [FromHeader(Name = "X-Push-Token")] string pushToken)
        {
            if (string.IsNullOrWhiteSpace(authToken) || string.IsNullOrWhiteSpace(username))
                return BadRequest(new { message = "Authentication token and username need to be supplied" });

            var firebaseUidFromToken = await _authService.VerifyTokenAsync(authToken);
            if (string.IsNullOrWhiteSpace(firebaseUidFromToken))
            {
                return Unauthorized();
            }

            try
            {
                var user = await _userRepo.CreateOrUpdateUserAsync(firebaseUidFromToken, pushToken, username);

                if (string.IsNullOrWhiteSpace(pushToken))
                {
                    if (!string.IsNullOrEmpty(user.PushRegistrationId))
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

                await _dataContext.SaveChangesAsync();

                var authInfo = JwtTokenHelper.CreateJwtToken(user.Id);
                Response.Headers.Add("WWW-Authenticate", $"Bearer {authInfo.Bearer}");
                return Ok(authInfo);
            }
            catch (UsernameTakenException)
            {
                return Conflict("Username already taken - please choose another one.");
            }
        }
    }
}