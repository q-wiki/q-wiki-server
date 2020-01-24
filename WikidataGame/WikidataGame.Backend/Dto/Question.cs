using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WikidataGame.Backend.Dto
{
    public class Question
    {
        public Guid Id { get; set; }

        [Required]
        public string SparqlQuery { get; set; }

        [Required]
        public string TaskDescription { get; set; }

        [Required]
        public virtual Category Category { get; set; }

        [Required]
        public MiniGameType MiniGameType { get; set; }

        public QuestionStatus Status { get; set; }

        public double Rating { get; set; }
    }

    public enum QuestionStatus
    {
        Pending,
        Rejected,
        Approved
    }
}
