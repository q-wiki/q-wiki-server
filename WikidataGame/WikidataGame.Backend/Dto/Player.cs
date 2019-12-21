using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WikidataGame.Backend.Dto
{
    public class Player
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string ProfileImage { get; set; }

    }
}
