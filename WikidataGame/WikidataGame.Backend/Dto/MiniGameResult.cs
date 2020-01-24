using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WikidataGame.Backend.Helpers;
using WikidataGame.Backend.Services;

namespace WikidataGame.Backend.Dto
{
    public class MiniGameResult
    {
        public bool IsWin { get; set; }

        public IEnumerable<string> CorrectAnswer { get; set; }

        public IEnumerable<IEnumerable<Tile>> Tiles { get; set; }

        public Guid? NextMovePlayerId { get; set; }

    }
}
