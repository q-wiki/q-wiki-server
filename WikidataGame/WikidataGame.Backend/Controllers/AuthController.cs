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

namespace WikidataGame.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : CustomControllerBase
    {
        public AuthController(
            DataContext dataContext,
            IUserRepository userRepo,
            IGameRepository gameRepo,
            IRepository<Models.Category, string> categoryRepo,
            INotificationService notificationService) : base(dataContext, userRepo, gameRepo, categoryRepo, notificationService)
        {}

        /// <summary>
        /// Authenticates a player 
        /// </summary>
        /// <param name="deviceId">device identifier</param>
        /// <param name="pushToken">push token generated through firebase/apns</param>
        /// <returns>Information on how to authenticate</returns>
        [HttpGet]
        [ProducesResponseType(typeof(AuthInfo), StatusCodes.Status200OK)]
        public async Task<IActionResult> Authenticate(
            [FromHeader(Name = "X-Device-ID")] string deviceId,
            [FromHeader(Name = "X-Push-Token")] string pushToken)
        {
            if (string.IsNullOrWhiteSpace(deviceId))
                return BadRequest(new { message = "DeviceId needs to be supplied" });

            var user = _userRepo.CreateOrUpdateUser(deviceId, pushToken);

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

            _dataContext.SaveChanges();

            var authInfo = JwtTokenHelper.CreateJwtToken(deviceId);
            Response.Headers.Add("WWW-Authenticate", $"Bearer {authInfo.Bearer}");
            return Ok(authInfo);
        }
    }
}