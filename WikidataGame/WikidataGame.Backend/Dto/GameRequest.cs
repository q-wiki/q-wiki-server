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
    }
}
