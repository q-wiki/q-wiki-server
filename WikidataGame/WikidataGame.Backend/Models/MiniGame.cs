using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WikidataGame.Backend.Models
{
    public class MiniGame
    {
        [Key]
        [StringLength(36)]
        public string Id { get; set; }

        [ForeignKey(nameof(Game))]
        [Required]
        [StringLength(36)]
        public string GameId { get; set; }
        public virtual Game Game { get; set; }

        [Required]
        public MiniGameType Type { get; set; }

        [Required]
        public string TaskDescription { get; set; }

        [NotMapped]
        public ICollection<string> AnswerOptions
        {
            get {
                return JsonConvert.DeserializeObject<List<string>>(AnswerOptionsString);
            }
            set
            {
                AnswerOptionsString = JsonConvert.SerializeObject(value);
            }
        }

        [Required]
        public string AnswerOptionsString { get; set; }

        [NotMapped]
        public ICollection<string> CorrectAnswer
        {
            get
            {
                return JsonConvert.DeserializeObject<List<string>>(CorrectAnswerString);
            }
            set
            {
                CorrectAnswerString = JsonConvert.SerializeObject(value);
            }
        }

        [Required]
        public string CorrectAnswerString { get; set; }

        [Required]
        public bool IsWin { get; set; } = false;

        [ForeignKey(nameof(User))]
        [StringLength(36)]
        [Required]
        public string PlayerId { get; set; }
        public virtual User Player { get; set; }
    }

    public enum MiniGameType
    {
        Sort,
        BlurryImage,
        MultipleChoice
    }
}
