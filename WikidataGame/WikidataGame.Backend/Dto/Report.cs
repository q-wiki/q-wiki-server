using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WikidataGame.Backend.Dto
{
    public class Report
    {
        public Guid Id { get; set; }

        [Required]
        public ProblemType ProblemType { get; set; }

        public Guid? MinigameId { get; set; }

        [Required]
        public MiniGameType MinigameType { get; set; }

        [Required]
        public string TaskDescription { get; set; }

        [Required]
        public string ProvidedAnswers { get; set; }

        public string AdditionalInformation { get; set; }
    }

    public enum ProblemType
    {
        Other,
        IncorrectAnswer,
        DuplicateAnswers
    }
}
