using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.NotificationHubs;

namespace Lunggo.Framework.Notification
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

            internal override bool SetTags(string registrationId, string notificationHandle, Platform platform, IEnumerable<string> tags)
            {
                var registration = CreateRegistration(notificationHandle, platform);
                registration.RegistrationId = registrationId;
                registration.Tags = new HashSet<string>(tags);

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

            internal override bool AddTags(string registrationId, string notificationHandle, Platform platform, IEnumerable<string> tags)
            {
                var registration = CreateRegistration(notificationHandle, platform);
                registration.RegistrationId = registrationId;
                var oldRegistration = _notificationHubClient.GetRegistrationAsync<RegistrationDescription>(registrationId).Result;
                registration.Tags = new HashSet<string>(oldRegistration.Tags.Concat(tags).Distinct());

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

            private static RegistrationDescription CreateRegistration(string notificationHandle, Platform platform)
            {
                switch (platform)
                {
                    case Platform.WindowsPhone:
                        return new MpnsRegistrationDescription(notificationHandle);
                        break;
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
        }
    }
}
