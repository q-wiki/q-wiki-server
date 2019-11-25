using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WikidataGame.Backend.Models;

namespace WikidataGame.Backend.Repos
{
    public interface IQuestionRepository : IRepository<Question, string>
    {
        Task<Question> GetRandomQuestionForCategoryAsync(string categoryId);
    }
}
