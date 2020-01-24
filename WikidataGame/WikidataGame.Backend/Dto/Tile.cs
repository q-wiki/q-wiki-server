using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WikidataGame.Backend.Helpers;
using WikidataGame.Backend.Repos;
using WikidataGame.Backend.Services;

namespace WikidataGame.Backend.Dto
{
    public class Tile
    {
        public Guid Id { get; set; }

        public Guid? ChosenCategoryId { get; set; }

        public IEnumerable<Category> AvailableCategories { get; set; }

        public int Difficulty { get; set; }

        public Guid? OwnerId { get; set; }

    }
}
