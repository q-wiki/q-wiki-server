using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WikidataGame.Backend.Models

{
    public class Game
    {
        [Key]
        public string Id { get; set; }

        public virtual ICollection<Tile> Tiles { get; set; }

        public virtual ICollection<User> Players { get; set; }

        [ForeignKey(nameof(User))]
        public string NextMovePlayerId { get; set; }
        public virtual User NextMovePlayer { get; set; }

        [ForeignKey(nameof(User))]
        public string WinningPlayerId { get; set; }
        public virtual User WinningPlayer { get; set; }
    }
}
