using System.Threading.Tasks;
using WikidataGame.Backend.Models;

namespace WikidataGame.Backend.Services
{
    public interface INotificationService
    {
        Task DeleteUserAsync(User user);
        Task<string> RegisterOrUpdatePushChannelAsync(User user, DeviceRegistration deviceRegistration);
        Task SendNotificationAsync(User receiver, string title, string body);
        Task SendDeleteNotificationAsync(User receiver, string title, string body);
        Task SendNotificationWithRefreshAsync(User receiver, string title, string body);
        Task SendRefreshNotificationAsync(User receiver);
    }
}