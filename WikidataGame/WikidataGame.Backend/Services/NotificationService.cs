using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.NotificationHubs;
using Microsoft.Azure.NotificationHubs.Messaging;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using WikidataGame.Backend.Models;

namespace WikidataGame.Backend.Services
{
    public class NotificationService : INotificationService
    {
        public const string UserIdTag = "userid";

        private readonly NotificationHubClient _hub;

        public NotificationService(string connectionString)
        {
            if (!string.IsNullOrWhiteSpace(connectionString))
            {
                _hub = NotificationHubClient.CreateClientFromConnectionString(
                    connectionString, "WikidataGameNotifications");
            }
        }

        public async Task SendNotificationAsync(User receiver, string title, string body)
        {
            var notificationObject = new
            {
                notification = new
                {
                    title = title,
                    body = body
                }
            };
            await SendNotificationAsync(receiver, notificationObject);
        }

        public async Task SendNotificationWithRefreshAsync(User receiver, string title, string body)
        {
            var notificationObject = new
            {
                notification = new
                {
                    title = title,
                    body = body
                },
                data = new
                {
                    refresh = true
                }
            };
            await SendNotificationAsync(receiver, notificationObject);
        }

        public async Task SendRefreshNotificationAsync(User receiver)
        {
            var notificationObject = new
            {
                data = new
                {
                    refresh = true
                }
            };
            await SendNotificationAsync(receiver, notificationObject);
        }

        private async Task SendNotificationAsync(User receiver, object content)
        {
            if (_hub == null || string.IsNullOrEmpty(receiver.PushRegistrationId))
                return;

            Notification notification;
            switch (receiver.Platform)
            {
                case GamePlatform.Android:
                    notification = new FcmNotification(JsonConvert.SerializeObject(content));
                    break;
                default:
                    throw new NotImplementedException();
            }

            await _hub.SendNotificationAsync(notification, $"{UserIdTag}:{receiver.Id}");
        }


        public async Task<string> RegisterOrUpdatePushChannelAsync(User user, DeviceRegistration deviceRegistration)
        {
            if (_hub == null)
                return string.Empty;

            var registrationId = user.PushRegistrationId;
            if (string.IsNullOrEmpty(registrationId))
            {
                registrationId = await _hub.CreateRegistrationIdAsync();
            }
            RegistrationDescription registration;
            switch (deviceRegistration.Platform)
            {
                case "mpns":
                    registration = new MpnsRegistrationDescription(deviceRegistration.Handle);
                    break;
                case "wns":
                    registration = new WindowsRegistrationDescription(deviceRegistration.Handle);
                    break;
                case "apns":
                    registration = new AppleRegistrationDescription(deviceRegistration.Handle);
                    break;
                case "fcm":
                    registration = new FcmRegistrationDescription(deviceRegistration.Handle);
                    break;
                default:
                    throw new NotImplementedException();
            }

            registration.RegistrationId = registrationId;

            registration.Tags = new HashSet<string>(deviceRegistration.Tags)
            {
                $"{UserIdTag}:{user.Id}"
            };

            try
            {
                await _hub.CreateOrUpdateRegistrationAsync(registration);
                return registrationId;
            }
            catch (MessagingException e)
            {
                //TODO: Handle properly
                return string.Empty;
            }

        }

        public async Task DeleteUserAsync(User user)
        {
            await _hub.DeleteRegistrationsByChannelAsync(user.PushRegistrationId);
        }
    }

    public class DeviceRegistration
    {
        public string Platform { get; set; }
        public string Handle { get; set; }
        public string[] Tags { get; set; }
    }
}
