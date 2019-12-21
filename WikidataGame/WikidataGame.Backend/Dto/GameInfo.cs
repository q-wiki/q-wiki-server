using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WikidataGame.Backend.Models;

namespace WikidataGame.Backend.Dto
{
    public class GameInfo
    {
        public Guid GameId { get; set; }
        public bool IsAwaitingOpponentToJoin { get; set; }
        public string Message { get; set; }
    }
}
