using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WikidataGame.Backend.Helpers;
using WikidataGame.Backend.Models;

namespace WikidataGame.Backend.Repos
{
    public class QuestionRepository : Repository<Question, Guid>, IQuestionRepository
    {
        public QuestionRepository(DataContext context) : base(context){}

        public Question GetRandomQuestionForCategory(Guid categoryId)
        {
            return Context.Set<Question>()
                .Where(q => q.Status == QuestionStatus.Approved && q.CategoryId == categoryId)
                .GroupBy(q => q.GroupId)
                .OrderBy(q => Guid.NewGuid())
                .First()
                .OrderBy(q => Guid.NewGuid())
                .First();
        }
    }
}
