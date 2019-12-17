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

        public async Task<Question> GetRandomQuestionForCategoryAsync(Guid categoryId)
        {
            return await Context.Set<Question>().Where(q => q.Status == QuestionStatus.Approved && q.CategoryId == categoryId)
                .OrderBy(q => Guid.NewGuid()).FirstOrDefaultAsync();
        }
    }
}
