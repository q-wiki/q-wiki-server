using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WikidataGame.Backend.Dto;
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
        private readonly IRepository<Models.Friend, Guid> _friendsRepo;
        public FriendsController(
            DataContext dataContext,
            IUserRepository userRepo,
            IGameRepository gameRepo,
            IRepository<Models.Friend, Guid> friendsRepo,
            INotificationService notificationService): base(dataContext, userRepo, gameRepo, notificationService)
        {
            _friendsRepo = friendsRepo;
        }

        /// <summary>
        /// Retrieves the friendlist for the signed in user
        /// </summary>
        /// <returns>List of friends</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Player>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Player>>> GetFriends()
        {
            var user = await GetCurrentUserAsync();
            var friends = await _friendsRepo.FindAsync(f => f.UserId == user.Id);
            return Ok(friends.Select(f => Player.FromModel(f)).ToList());
        }
        

        /// <summary>
        /// Adds a user as a friend
        /// </summary>
        /// <param name="friendId">The user id of the player that should be added</param>
        /// <returns>Details of the added user</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Player), StatusCodes.Status201Created)]
        public async Task<ActionResult<Player>> PostFriend(Guid friendId)
        {
            var user = await GetCurrentUserAsync();
            var friendUser = await _userRepo.GetAsync(friendId);
            if(friendUser == null) //user does not exist
            {
                NotFound();
            }

            if((await _friendsRepo.SingleOrDefaultAsync(f => f.UserId == user.Id && f.FriendId == friendId)) != null) // already friends
            {
                BadRequest();
            }

            var friend = new Models.Friend
            {
                UserId = user.Id,
                FriendId = friendId,
            };
            await _friendsRepo.AddAsync(friend);
            await _dataContext.SaveChangesAsync();

            return Created("", Player.FromModel(friend));
        }

        /// <summary>
        /// Removes a friend from the friend list
        /// </summary>
        /// <param name="friendId">User id of the friend to be removed</param>
        /// <returns></returns>
        [HttpDelete("{friendId}")]
        [ProducesResponseType(typeof(Player), StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteFriend(Guid friendId)
        {
            var user = await GetCurrentUserAsync();
            var friendship = await _friendsRepo.SingleOrDefaultAsync(f => f.UserId == user.Id && f.FriendId == friendId);
            if (friendship == null) // not friends
            {
                BadRequest();
            }

            _friendsRepo.Remove(friendship);
            await _dataContext.SaveChangesAsync();

            return NoContent();
        }

    }
}
