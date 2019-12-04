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

        public static MiniGame FromModel(Models.MiniGame minigame)
        {
            return new MiniGame
            {
                Id = minigame.Id,
                TaskDescription = minigame.TaskDescription,
                AnswerOptions = minigame.AnswerOptions,
                Type = (MiniGameType)Enum.Parse(typeof(MiniGameType), minigame.Type.ToString())
            };
        }
    }

    public enum MiniGameType
    {
        Sort,
        BlurryImage,
        MultipleChoice
    }
}
