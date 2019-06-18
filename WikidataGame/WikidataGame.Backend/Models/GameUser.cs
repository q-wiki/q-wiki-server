using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WikidataGame.Backend.Models
{
    public class GameUser
    {
        //Has composed primary key of GameId and UserId
        [StringLength(36)]
        public string GameId { get; set; }
        public virtual Game Game { get; set; }


        [StringLength(36)]
        public string UserId { get; set; }
        public virtual User User { get; set; }

        public bool IsWinner { get; set; } = false;
    }
}
