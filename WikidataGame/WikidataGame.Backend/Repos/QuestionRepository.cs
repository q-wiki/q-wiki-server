using Microsoft.EntityFrameworkCore;
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

        public async Task<Question> GetRandomQuestionForCategoryAsync(string categoryId)
        {
            return await Context.Set<Question>().Where(q => q.CategoryId == categoryId)
                .OrderBy(q => Guid.NewGuid()).FirstOrDefaultAsync();
        }
    }
}
