using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WikidataGame.Backend.Dto
{
    public class Question
    {
        public Guid Id { get; set; }

        public string SparqlQuery { get; set; }

        public string TaskDescription { get; set; }

        public virtual Category Category { get; set; }

        public MiniGameType MiniGameType { get; set; }

        public double Rating { get; set; }
    }
}
