using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WikidataGame.Backend.Helpers;
using WikidataGame.Backend.Models;

namespace WikidataGame.Backend.Repos
{
    public class QuestionRepository : Repository<Question, string>, IQuestionRepository
    {
        public QuestionRepository(DataContext context) : base(context){}

        public Question GetRandomQuestionForMinigameType(MiniGameType type)
        {
            return (Context as DataContext)
                .Questions.Where(q => q.MiniGameType == type)
                .OrderBy(q => Guid.NewGuid()).FirstOrDefault();
        }
    }
}
