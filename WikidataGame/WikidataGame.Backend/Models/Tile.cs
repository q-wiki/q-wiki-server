using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WikidataGame.Backend.Models
{
    public class Tile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [ForeignKey(nameof(Category))]
        public Guid? ChosenCategoryId { get; set; }
        public virtual Category ChosenCategory { get; set; }
       
        public int Difficulty { get; set; }

        [ForeignKey(nameof(User))]
        public Guid? OwnerId { get; set; }
        public virtual User Owner { get; set; }

        [Required]
        public bool IsAccessible { get; set; }

        public override string ToString() => IsAccessible ? Difficulty.ToString() : " ";

        // Equals override taken from https://stackoverflow.com/a/567648
        public override bool Equals ( object obj )
        {
            return Equals(obj as Tile);
        }

        public bool Equals ( Tile other )
        {
            return other != null && other.Id == this.Id
                && other.ChosenCategoryId == this.ChosenCategoryId
                && other.Difficulty == this.Difficulty
                && other.OwnerId == this.OwnerId
                && other.IsAccessible == this.IsAccessible; 
        }
    }
}