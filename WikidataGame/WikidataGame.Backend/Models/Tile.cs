using System.ComponentModel.DataAnnotations;

namespace WikidataGame.Backend.Models
{
    public class Tile
    {
        [Key]
        public string Id { get; set; }

        public virtual Category ChosenCategory { get; set; }
        public string ChosenCategoryId { get; set; }
        public virtual Category[] AvailableCategories { get; set; }

        public int Difficulty { get; set; }

        public virtual User Owner { get; set; }
        public string OwnerId { get; set; }

        public bool IsAccessible { get; set; }

        public override string ToString() => IsAccessible ? Difficulty.ToString() : " ";
    }
}