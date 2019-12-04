using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WikidataGame.Backend.Dto
{
    public class Player
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string ProfileImage { get; set; }

        public static Player FromModel(Models.User player)
        {
            if (player == null)
                return null;

            return new Player
            {
                Id = player.Id,
                Name = player.UserName,
                ProfileImage = player.ProfileImageUrl
            };
        }

        public static Player FromModel(Models.Friend friend)
        {
            if (friend == null)
                return null;

            return FromModel(friend.FriendUser);
        }
    }
}
