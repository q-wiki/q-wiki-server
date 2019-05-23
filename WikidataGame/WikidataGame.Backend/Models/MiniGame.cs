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
        public string Id { get; set; }

        public MiniGameType Type { get; set; }

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

        public string CorrectAnswerString { get; set; }

        public bool IsWin { get; set; } = false;

        [ForeignKey(nameof(User))]
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
