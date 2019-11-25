using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WikidataGame.Backend.Helpers;
using WikidataGame.Backend.Models;
using WikidataGame.Backend.Repos;
using WikidataGame.Backend.Services;

namespace WikidataGame.Backend.Controllers
{
    public class CustomControllerBase : ControllerBase
    {
        protected readonly IGameRepository _gameRepo;
        protected readonly IUserRepository _userRepo;
        protected readonly IRepository<Category, string> _categoryRepo;
        protected readonly DataContext _dataContext;
        protected readonly INotificationService _notificationService;

        public CustomControllerBase(
            DataContext dataContext,
            IUserRepository userRepo,
            IGameRepository gameRepo,
            IRepository<Category, string> categoryRepo,
            INotificationService notificationService)
        {
            _dataContext = dataContext;
            _userRepo = userRepo;
            _gameRepo = gameRepo;
            _categoryRepo = categoryRepo;
            _notificationService = notificationService;
        }

        protected async Task<User> GetCurrentUserAsync()
        {
            return await _userRepo.SingleOrDefaultAsync(u => u.Id == new Guid(User.Identity.Name));
        }

        protected async Task<bool> IsUserGameParticipantAsync(Guid gameId)
        {
            var user = await GetCurrentUserAsync();
            var game = await _gameRepo.GetAsync(gameId);
            return game != null && game.GameUsers.SingleOrDefault(gu => gu.UserId == user.Id) != null;
        }
    }
}
