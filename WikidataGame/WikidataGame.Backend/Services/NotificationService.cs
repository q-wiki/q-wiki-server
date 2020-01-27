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
using Newtonsoft.Json.Linq;
using WikidataGame.Backend.Models;

namespace WikidataGame.Backend.Services
{
    public class NotificationService : INotificationService
    {
        public const string UserIdTag = "userid";

        public Dictionary<PushType, PushTemplate> PushTemplates { get; } = new Dictionary<PushType, PushTemplate>
        {
            {
                PushType.Delete,
                new PushTemplate {
                    Title = "Congrats",
                    Body = "You won against {0} because {0} left the game!",
                    Data = new PushData { Action = "won" }
                }
            },
            {
                PushType.YouLost,
                new PushTemplate {
                    Title = "Too bad!",
                    Body = "You lost against {0}! Start a new game for another chance.",
                    Data = new PushData { Action = "lost" }
                }
            },
            {
                PushType.YouLostTimeout,
                new PushTemplate {
                    Title = "Too bad!",
                    Body = "You lost against {0} due to inactivity!",
                    Data = new PushData { Action = "lost" }
                }
            },
            {
                PushType.YourTurn,
                new PushTemplate {
                    Title = "It's your turn!",
                    Body = "You have 12 hours left to play your round against {0}.",
                    Data = new PushData { Action = "refresh" }
                }
            },
            {
                PushType.YouWon,
                new PushTemplate {
                    Title = "Congrats",
                    Body = "You won against {0} on points!",
                    Data = new PushData { Action = "won" }
                }
            },
            {
                PushType.YouWonTimeout,
                new PushTemplate {
                    Title = "Congrats",
                    Body = "You won against {0} because {0} was inactive!",
                    Data = new PushData { Action = "won" }
                }
            },
            {
                PushType.GameRequest,
                new PushTemplate
                {
                    Title = "New game request",
                    Body = "{0} wants to challenge you! Open Q-Wiki to accept the request.",
                    Data = new PushData { Action = "request" }
                }
            },
            {
                PushType.Draw,
                new PushTemplate
                {
                    Title = "Draw!",
                    Body = "Your game against {0} ended in a draw.",
                    Data = new PushData { Action = "draw" }
                }
            }
        };

        private readonly NotificationHubClient _hub;

        public NotificationService(string connectionString)
        {
            if (!string.IsNullOrWhiteSpace(connectionString))
            {
                _hub = NotificationHubClient.CreateClientFromConnectionString(
                    connectionString, "WikidataGameNotifications");
            }
        }

        public async Task SendNotificationAsync(PushType type, User recipient, User opponent, Guid? gameId = null)
        {
            PushTemplates.TryGetValue(type, out var template);
            if(gameId.HasValue)
                template.Data.GameId = gameId.Value;

            var notificationObject = new
            {
                notification = new
                {
                    title = string.Format(template.Title, opponent.UserName),
                    body = string.Format(template.Body, opponent.UserName)
                },
                data = template.Data
            };
            await SendNotificationAsync(recipient, notificationObject);
        }

        private async Task SendNotificationAsync(User receiver, object content)
        {
            if (_hub == null || string.IsNullOrEmpty(receiver.PushRegistrationId))
                return;

            Notification notification;
            switch (receiver.PushPlatform)
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
            catch (MessagingException)
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

    public enum PushType
    {
        YourTurn,
        YouLost,
        YouWon,
        YouLostTimeout,
        YouWonTimeout,
        Draw,
        Delete,
        GameRequest
    }

    public class PushTemplate
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public PushData Data { get; set; }
    }

    public class PushData
    {
        public string Action { get; set; }
        public Guid GameId { get; set; }
    }

}
