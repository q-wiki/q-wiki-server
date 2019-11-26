using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WikidataGame.Backend.Models;
using WikidataGame.Backend.Repos;

namespace WikidataGame.Backend.Services
{
    public class CategoryCacheService
    {
        private readonly IRepository<Category, Guid> _categoryRepo;
        private IEnumerable<Category> _categories;
        public CategoryCacheService(IRepository<Category, Guid> categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            if (_categories == null)
                _categories = await _categoryRepo.GetAllAsync();
            return _categories;
        }
    }
}
