using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WikidataGame.Backend.Dto;
using WikidataGame.Backend.Helpers;
using WikidataGame.Backend.Models;
using WikidataGame.Backend.Repos;

namespace WikidataGame.Backend.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GamesController : ControllerBase
    {
        private readonly IRepository<User,string> _userRepo;
        private readonly AppSettings _appSettings;
        private readonly DataContext _dataContext;

        public GamesController(
            DataContext dataContext,
            IRepository<User, string> userRepo,
            IOptions<AppSettings> appSettings)
        {
            _dataContext = dataContext;
            _userRepo = userRepo;
            _appSettings = appSettings.Value;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult CreateNewGame(
            [FromHeader(Name = "X-Device-ID")] string deviceId,
            [FromHeader(Name = "X-Push-URL")] string pushUrl)
        {
            if(string.IsNullOrWhiteSpace(deviceId))
                return BadRequest(new { message = "DeviceId needs to be supplied" });

            var user = _userRepo.Get(deviceId);
            if (user == null)
            {
                _userRepo.Add(new User
                {
                    DeviceId = deviceId,
                    PushChannelUrl = pushUrl
                });
            }
            else
            {
                user.PushChannelUrl = pushUrl;
                _userRepo.Update(user);
            }
            _dataContext.SaveChanges();

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, deviceId)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            Response.Headers.Add("WWW-Authenticate", $"Bearer {tokenString}");
            return Ok (new GameInfo
            {
                GameId = Guid.NewGuid().ToString(),
                IsAwaitingOpponentToJoin = true,
                Message = "Hello World!"
            });
        }
    }
}