using System;
using System.Threading.Tasks;
using WikidataGame.Backend.Models;

namespace WikidataGame.Backend.Services
{
    public interface INotificationService
    {
        Task DeleteUserAsync(User user);
        Task<string> RegisterOrUpdatePushChannelAsync(User user, DeviceRegistration deviceRegistration);
        Task SendNotificationAsync(PushType type, User recipient, User opponent, Guid gameId);
    }
}