using System.Threading.Tasks;
using WikidataGame.Backend.Models;

namespace WikidataGame.Backend.Services
{
    public interface INotificationService
    {
        Task DeleteUserAsync(User user);
        Task<string> RegisterOrUpdatePushChannelAsync(User user, DeviceRegistration deviceRegistration);
        Task SendNotificationAsync(User receiver, string title, string body);
        Task SendNotificationWithDataAsync(User receiver, string title, string body, object data);
        Task SendSilentNotificationAsync(User receiver, object data);
    }
}