using AutoMapper;
using Microsoft.AspNetCore.Identity;
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
        protected readonly UserManager<User> _userManager;
        protected readonly IRepository<Category, Guid> _categoryRepo;
        protected readonly DataContext _dataContext;
        protected readonly INotificationService _notificationService;
        protected readonly IMapper _mapper;

        public CustomControllerBase(
            DataContext dataContext,
           UserManager<User> userManager,
            IGameRepository gameRepo,
            INotificationService notificationService,
            IMapper mapper)
        {
            _dataContext = dataContext;
            _userManager = userManager;
            _gameRepo = gameRepo;
            _notificationService = notificationService;
            _mapper = mapper;
        }

        protected async Task<User> GetCurrentUserAsync()
        {
            return await _userManager.GetUserAsync(User);
        }

        protected async Task<bool> IsUserGameParticipantAsync(Guid gameId)
        {
            var user = await GetCurrentUserAsync();
            var game = await _gameRepo.GetAsync(gameId);
            return game != null && game.GameUsers.SingleOrDefault(gu => gu.UserId == user.Id) != null;
        }
    }
}
