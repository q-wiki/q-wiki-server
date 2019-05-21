using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WikidataGame.Backend.Dto
{
    public class Player
    {
        public string Id { get; set; }
        public string DeviceId { get; set; }
        public string Name { get; set; }

        public static Player FromModel(Models.User player)
        {
            if (player == null)
                return null;

            return new Player
            {
                Id = player.DeviceId,
                DeviceId = player.DeviceId,
                Name = $"Player {player.DeviceId}"
            };
        }
    }
}
