using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WikidataGame.Backend.Models
{
    public class GameUser
    {
        //Has composed primary key of GameId and UserId (see DataContext)
        [ForeignKey(nameof(Game))]
        public Guid GameId { get; set; }
        public virtual Game Game { get; set; }

        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }
        public virtual User User { get; set; }

        public bool IsWinner { get; set; } = false;
    }
}
