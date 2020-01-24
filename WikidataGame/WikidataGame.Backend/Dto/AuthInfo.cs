using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WikidataGame.Backend.Dto
{
    public class AuthInfo
    {
        public string Bearer { get; set; }
        public DateTime Expires { get; set; }
        public Player User { get; set; }
    }
}
