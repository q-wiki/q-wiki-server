using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WikidataGame.Backend.Dto
{
    public class MiniGame
    {
        public Guid Id { get; set; }

        public MiniGameType Type { get; set; }

        public string TaskDescription { get; set; }

        public IEnumerable<string> AnswerOptions { get; set; }

        public string ImageUrl { get; set; }

        public string LicenseInfo { get; set; }
    }

    public enum MiniGameType
    {
        Sort,
        Image,
        MultipleChoice
    }

    public class DetailedMiniGame : MiniGame
    {
        public Question Question { get; set; }

        public IEnumerable<string> CorrectAnswer { get; set; }
    }
}
