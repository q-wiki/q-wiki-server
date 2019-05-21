using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WikidataGame.Backend.Models
{
    public class Tile
    {
        [Key]
        public string Id { get; set; }

        [ForeignKey(nameof(Category))]
        public string ChosenCategoryId { get; set; }
        public virtual Category ChosenCategory { get; set; }

        public ICollection<Category> AvailableCategories { get; set; }

        public int Difficulty { get; set; }

        [ForeignKey(nameof(User))]
        public string OwnerId { get; set; }
        public virtual User Owner { get; set; }

        public bool IsAccessible { get; set; }

        public override string ToString() => IsAccessible ? Difficulty.ToString() : " ";
    }
}