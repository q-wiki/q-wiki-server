using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WikidataGame.Backend.Helpers;
using WikidataGame.Backend.Models;
using WikidataGame.Backend.Services;

namespace WikidataGame.Backend.Dto
{
    public class Game
    {
        public Guid Id { get; set; }

        public IEnumerable<IEnumerable<Tile>> Tiles { get; set; }

        public IEnumerable<Guid> WinningPlayerIds { get; set; }

        public Player Me { get; set; }

        public Player Opponent { get; set; }

        public Guid? NextMovePlayerId { get; set; }

        public DateTime? MoveExpiry { get; set; }

        public bool AwaitingOpponentToJoin { get; set; }

    }
}
