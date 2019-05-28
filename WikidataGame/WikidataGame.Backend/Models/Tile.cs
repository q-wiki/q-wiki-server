using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WikidataGame.Backend.Models
{
    public class Tile
    {
        [Key]
        [StringLength(36)]
        public string Id { get; set; }

        [ForeignKey(nameof(Category))]
        [StringLength(36)]
        public string ChosenCategoryId { get; set; }
        public virtual Category ChosenCategory { get; set; }
       
        public int Difficulty { get; set; }

        [ForeignKey(nameof(User))]
        [StringLength(36)]
        public string OwnerId { get; set; }
        public virtual User Owner { get; set; }

        [Required]
        public bool IsAccessible { get; set; }

        public override string ToString() => IsAccessible ? Difficulty.ToString() : " ";
    }
}