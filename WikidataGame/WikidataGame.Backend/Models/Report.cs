using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WikidataGame.Backend.Models
{
    public class Report
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public ProblemType ProblemType { get; set; }
        
        public Guid? MinigameId { get; set; }
        [ForeignKey(nameof(MinigameId))]
        public virtual MiniGame Minigame { get; set; }

        public MiniGameType MinigameType { get; set; }
        public string TaskDescription { get; set; }
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
