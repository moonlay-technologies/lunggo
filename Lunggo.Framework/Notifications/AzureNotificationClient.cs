using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Extension;
using Microsoft.Azure.NotificationHubs;

namespace Lunggo.Apcommon.Notifications
{
    public partial class NotificationService
    {
        private class AzureNotificationClient : NotificationClient
        {
            private static readonly AzureNotificationClient ClientInstance = new AzureNotificationClient();
            private bool _isInitialized;
            private NotificationHubClient _notificationHubClient;

            private AzureNotificationClient()
            {

            }

            internal static AzureNotificationClient GetClientInstance()
            {
                return ClientInstance;
            }

            internal override void Init(string connString, string hubName)
            {
                if (!_isInitialized)
                {
                    _notificationHubClient = NotificationHubClient.CreateClientFromConnectionString(connString, hubName);
                    _isInitialized = true;
                }
            }

            internal override string RegisterDevice(string notificationHandle, string deviceId)
            {
                string newRegistrationId = null;

                // make sure there are no existing registrations for this push handle (used for iOS and Android)
                if (notificationHandle != null)
                {
                    var registrations = _notificationHubClient.GetRegistrationsByChannelAsync(notificationHandle, 100).Result;

                    foreach (RegistrationDescription registration in registrations)
                    {
                        if (newRegistrationId == null)
                        {
                            newRegistrationId = registration.RegistrationId;
                        }
                        else
                        {
                            _notificationHubClient.DeleteRegistrationAsync(registration).Wait();
                        }
                    }
                }

                if (newRegistrationId == null)
                    newRegistrationId = _notificationHubClient.CreateRegistrationIdAsync().Result;

                return newRegistrationId;
            }

            internal override bool UpdateTags(string registrationId, string notificationHandle, Platform platform, Dictionary<string, string> newTags)
            {
                var registration = CreateRegistration(notificationHandle, platform);
                registration.RegistrationId = registrationId;
                var oldRegistration = _notificationHubClient.GetRegistrationAsync<RegistrationDescription>(registrationId).Result;
                Dictionary<string, string> tags;
                if (oldRegistration != null)
                {
                    tags = DeserializeTags(oldRegistration.Tags);
                    foreach (var newTag in newTags)
                    {
                        tags[newTag.Key] = newTag.Value;
                    }
                }
                else
                {
                    tags = newTags;
                }
                registration.Tags = new HashSet<string>(SerializeTags(tags));

                try
                {
                    _notificationHubClient.CreateOrUpdateRegistrationAsync(registration).Wait();
                }
                catch
                {
                    return false;
                }

                return true;
            }

            internal override void DeleteRegistration(string registrationId)
            {
                _notificationHubClient.DeleteRegistrationAsync(registrationId).Wait();
            }

            internal override void PushNotification(Notification notification, Dictionary<string, string> tags)
            {
                _notificationHubClient.SendAppleNativeNotificationAsync(ConstructApplePayload(notification), SerializeTags(tags));
                _notificationHubClient.SendGcmNativeNotificationAsync(ConstructGcmPayload(notification), SerializeTags(tags));
            }

            internal override void PushSilentNotification(object data, Dictionary<string, string> tags)
            {
                _notificationHubClient.SendAppleNativeNotificationAsync(ConstructApplePayload(data), SerializeTags(tags));
                _notificationHubClient.SendGcmNativeNotificationAsync(ConstructGcmPayload(data), SerializeTags(tags));
            }

            private static string ConstructGcmPayload(Notification notification)
            {
                var payload = new
                {
                    title = notification.Title,
                    message = notification.Message,
                    image = notification.Image,
                    icon = notification.Icon,
                    code = notification.Code,
                    data = notification.CustomData
                };
                return payload.Serialize();
            }

            private static string ConstructGcmPayload(object data)
            {
                var payload = new {data};
                return payload.Serialize();
            }

            private static string ConstructApplePayload(Notification notification)
            {
                var payload = new
                {
                    aps = new
                    {
                        alert = new
                        {
                            title = notification.Title,
                            body = notification.Message
                        }
                    },
                    code = notification.Code,
                    data = notification.CustomData
                };
                return payload.Serialize();
            }

            private static string ConstructApplePayload(object data)
            {
                var payload = new {data};
                return payload.Serialize();
            }

            private static RegistrationDescription CreateRegistration(string notificationHandle, Platform platform)
            {
                switch (platform)
                {
                    case Platform.Ios:
                        return new AppleRegistrationDescription(notificationHandle);
                        break;
                    case Platform.Android:
                        return new GcmRegistrationDescription(notificationHandle);
                        break;
                    default:
                        return null;
                }
            }

            private static IEnumerable<string> SerializeTags(Dictionary<string, string> tags)
            {
                return tags.Select(tag => tag.Key + ":" + tag.Value);
            }

            private static Dictionary<string, string> DeserializeTags(IEnumerable<string> tags)
            {
                return tags.Select(tag => tag.Split(':')).ToDictionary(splitTag => splitTag[0], splitTag => splitTag[1]);
            }
        }
    }
}
