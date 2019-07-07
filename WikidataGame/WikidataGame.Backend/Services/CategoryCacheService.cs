﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WikidataGame.Backend.Models;
using WikidataGame.Backend.Repos;

namespace WikidataGame.Backend.Services
{
    public class CategoryCacheService
    {
        public IEnumerable<Category> Categories { get; private set; }
        public CategoryCacheService(IRepository<Category, string> categoryRepo)
        {
            Categories = categoryRepo.GetAll();
        }
    }
}
