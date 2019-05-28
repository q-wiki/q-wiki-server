using System.ComponentModel.DataAnnotations;

namespace WikidataGame.Backend.Models
{
    public class Category
    {
        [Key]
        [StringLength(36)]
        public string Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }
    }
}