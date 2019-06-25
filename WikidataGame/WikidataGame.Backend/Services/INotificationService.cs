using System.Threading.Tasks;
using WikidataGame.Backend.Models;

namespace WikidataGame.Backend.Services
{
    public interface INotificationService
    {
        Task DeleteUser(User user);
        Task<string> RegisterOrUpdatePushChannel(User user, DeviceRegistration deviceRegistration);
        Task SendNotification(User receiver, string title, string body);
    }
}