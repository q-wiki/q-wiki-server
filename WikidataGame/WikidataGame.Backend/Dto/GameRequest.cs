using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WikidataGame.Backend.Dto
{
    public class GameRequest
    {
        public Guid Id { get; set; }
        public Player Sender { get; set; }
        public Player Recipient { get; set; }

        public static GameRequest FromModel(Models.GameRequest gameRequest)
        {
            if (gameRequest == null)
                return null;

            return new GameRequest
            {
                Id = gameRequest.Id,
                Sender = Player.FromModel(gameRequest.Sender),
                Recipient = Player.FromModel(gameRequest.Recipient)
            };
        }
    }
}
