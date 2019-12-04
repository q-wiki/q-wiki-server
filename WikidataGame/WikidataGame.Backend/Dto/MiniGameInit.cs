using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WikidataGame.Backend.Dto
{
    public class MiniGameInit
    {
        public Guid TileId { get; set; }

        public Guid CategoryId { get; set; }
    }
}
