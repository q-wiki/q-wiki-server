using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WikidataGame.Backend.Dto
{
    public class MiniGameResult
    {
        public bool IsWin { get; set; }

        public IEnumerable<string> CorrectAnswer { get; set; }

        public IEnumerable<Tile> Tiles { get; set; }

        public string NextMovePlayerId { get; set; }
    }
}
