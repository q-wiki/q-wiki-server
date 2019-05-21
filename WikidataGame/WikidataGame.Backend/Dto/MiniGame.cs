﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WikidataGame.Backend.Dto
{
    public class MiniGame
    {
        public string Id { get; set; }

        public MiniGameType Type { get; set; }

        public string TaskDescription { get; set; }

        public IEnumerable<string> AnswerOptions { get; set; }
    }

    public enum MiniGameType
    {
        Sort,
        BlurryImage,
        MultipleChoice
    }
}