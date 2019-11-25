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
        [NotMapped]
        public const int StepsPerPlayer = 3;

        [NotMapped]
        public const int MaxRounds = 6;

        [NotMapped]
        public static readonly TimeSpan MaxMoveDuration = TimeSpan.FromHours(12);

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public virtual ICollection<Tile> Tiles { get; set; } = new List<Tile>();

        public int MapWidth { get; set; }
        public int MapHeight { get; set; }
        public int AccessibleTilesCount { get; set; }

        public virtual ICollection<GameUser> GameUsers { get; set; } = new List<GameUser>();

        [ForeignKey(nameof(User))]
        public Guid? NextMovePlayerId { get; set; }
        public virtual User NextMovePlayer { get; set; }

        public int StepsLeftWithinMove { get; set; }

        public int MoveCount { get; set; }

        public DateTime? MoveStartedAt { get; set; }
    }
}
