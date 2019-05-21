using System.ComponentModel.DataAnnotations;

namespace WikidataGame.Backend.Models
{
    public class Category
    {
        [Key]
        public string Id { get; set; }

        public string Title { get; set; }
    }
}