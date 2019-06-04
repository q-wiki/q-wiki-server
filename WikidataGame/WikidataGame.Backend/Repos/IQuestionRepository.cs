﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WikidataGame.Backend.Models;

namespace WikidataGame.Backend.Repos
{
    public interface IQuestionRepository : IRepository<Question, string>
    {
        Question GetRandomQuestionForMinigameType(MiniGameType type, string categoryId);
    }
}