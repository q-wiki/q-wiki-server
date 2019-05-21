using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WikidataGame.Backend.Dto
{
    public class Tile
    {
        public string Id { get; set; }

        public string ChosenCategoryId { get; set; }

        public IEnumerable<Category> AvailableCategories { get; set; }

        public int Difficulty { get; set; }

        public string OwnerId { get; set; }
    }
}
