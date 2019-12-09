using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WikidataGame.Backend.Dto
{
    public class GameRequestList
    {
        public IEnumerable<GameRequest> Incoming { get; set; }
        public IEnumerable<GameRequest> Outgoing { get; set; }
    }
}
