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

        public static Player FromModel(Models.User player)
        {
            if (player == null)
                return null;

            return new Player
            {
                Id = player.Id,
                Name = player.Username
            };
        }

        public static Player FromModel(Models.Friend friend)
        {
            if (friend == null)
                return null;

            return new Player
            {
                Id = friend.FriendId,
                Name = friend.FriendUser.Username
            };
        }
    }
}
