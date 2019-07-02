using Microsoft.Azure.WebJobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikidataGame.Backend.Helpers;
using WikidataGame.Backend.Models;
using WikidataGame.Backend.Repos;
using WikidataGame.Backend.Services;

namespace WikidataGame.Backend.WebJob
{
    public class Functions
    {
        private readonly IGameRepository _gameRepo;
        private readonly INotificationService _notificationService;
        private readonly DataContext _dataContext;

        public Functions(IGameRepository gameRepo, INotificationService notificationService, DataContext dataContext)
        {
            _gameRepo = gameRepo;
            _notificationService = notificationService;
            _dataContext = dataContext;
        }

        public async Task ExpiryCleanup([TimerTrigger("0 */5 * * * *", RunOnStartup = true, UseMonitor = true)]TimerInfo timerInfo)
        {
            Console.WriteLine("Executing web job ....");
            var expiredGames = _gameRepo.Find(x => x.GameUsers.Count(gu => gu.IsWinner) <= 0 &&
                x.MoveStartedAt.HasValue && x.MoveStartedAt.Value.Add(Game.MaxMoveDuration) < DateTime.UtcNow).ToList();
            foreach(var game in expiredGames)
            {
                var expiringPlayer = game.GameUsers.SingleOrDefault(gu => gu.UserId == game.NextMovePlayerId);
                var winningPlayer = game.GameUsers.SingleOrDefault(gu => gu.UserId != game.NextMovePlayerId);
                winningPlayer.IsWinner = true;
                await _notificationService.SendNotification(winningPlayer.User, "Congrats", "You won this game due to your opponent being inactive!");
                await _notificationService.SendNotification(expiringPlayer.User, "Too bad.", "You lost the game due to inactivity!");
            }
            await _dataContext.SaveChangesAsync();
        }
    }
}
