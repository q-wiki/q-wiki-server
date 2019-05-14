using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WikidataGame.Backend.Helpers;
using WikidataGame.Backend.Models;
using WikidataGame.Backend.Repos;

namespace WikidataGame.Backend.Controllers
{
    public class CustomControllerBase : ControllerBase
    {
        protected readonly IRepository<User, string> _userRepo;
        protected readonly DataContext _dataContext;

        public CustomControllerBase(
            DataContext dataContext,
            IRepository<User, string> userRepo)
        {
            _dataContext = dataContext;
            _userRepo = userRepo;
        }

        protected User GetCurrentUser()
        {
            return _userRepo.Get(User.Identity.Name);
        }
    }
}
