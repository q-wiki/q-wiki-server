using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WikidataGame.Backend.Models
{
    public class Question
    {
        [Key]
        public string Id { get; set; }

        public string SparqlQuery { get; set; }

        public string TaskDescription { get; set; }

        [ForeignKey(nameof(Category))]
        public string CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public MiniGameType MiniGameType { get; set; }
    }
}
