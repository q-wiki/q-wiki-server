using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WikidataGame.Backend.Helpers;
using WikidataGame.Backend.Repos;
using WikidataGame.Backend.Services;

namespace WikidataGame.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FriendsController : CustomControllerBase
    {
        private readonly IRepository<Models.Friend, Guid> _friendRepo;
        public FriendsController(
            DataContext dataContext,
            IUserRepository userRepo,
            IGameRepository gameRepo,
            IRepository<Models.Category, string> categoryRepo,
            INotificationService notificationService): base(dataContext, userRepo, gameRepo, categoryRepo, notificationService)
        {
        }

        // GET: api/Friends
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.Friend>>> GetFriends()
        {
            return await _dataContext.Friends.ToListAsync();
        }
        

        // POST: api/Friends
        [HttpPost]
        public async Task<ActionResult<Models.Friend>> PostFriend(Models.Friend friend)
        {
            _dataContext.Friends.Add(friend);
            await _dataContext.SaveChangesAsync();

            return CreatedAtAction("GetFriend", new { id = friend.RelationId }, friend);
        }

        // DELETE: api/Friends/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Models.Friend>> DeleteFriend(string id)
        {
            var friend = await _dataContext.Friends.FindAsync(id);
            if (friend == null)
            {
                return NotFound();
            }

            _dataContext.Friends.Remove(friend);
            await _dataContext.SaveChangesAsync();

            return friend;
        }

    }
}
