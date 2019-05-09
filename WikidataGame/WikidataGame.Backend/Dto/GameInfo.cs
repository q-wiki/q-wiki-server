using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WikidataGame.Backend.Dto
{
    public class GameInfo
    {
        public string GameId { get; set; }
        public bool IsAwaitingOpponentToJoin { get; set; }
        public string Message { get; set; }
    }
}
