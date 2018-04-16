using Lunggo.Framework.Database;
using System.Collections.Generic;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;

namespace Lunggo.Framework.Notifications
{
    public partial class NotificationService
    {
        private static readonly NotificationService Instance = new NotificationService();
        private bool _isInitialized;
        private static readonly AzureNotificationClient Client = AzureNotificationClient.GetClientInstance();

        private NotificationService()
        {

        }
        public void Init(string connString, string hubName)
        {
            if (!_isInitialized)
            {
                Client.Init(connString, hubName);
                _isInitialized = true;
            }
        }
        public static NotificationService GetInstance()
        {
            return Instance;
        }

        public string RegisterDevice(string notificationHandle, string deviceId)
        {
            //return Client.RegisterDevice(notificationHandle, deviceId);
            return RegisterDeviceExpoToDb(notificationHandle, deviceId);
        }

        internal string RegisterDeviceExpoToDb(string notificationHandle, string deviceId)
        {
            var RegisterId = "";
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var notificationRecord = new NotificationTableRecord
                {
                    UserId = user,
                    ActivityId = activityId
                };
                try
                {
                    WishlistTableRepo.GetInstance().Insert(conn, wishlistRecord);
                }
                catch
                {

                }
            }
            return RegisterId;
        }

        public bool UpdateTags(string registrationId, string notificationHandle, Platform platform,
            Dictionary<string, string> tags)
        {
            return Client.UpdateTags(registrationId, notificationHandle, platform, tags);
        }

        public void DeleteRegistration(string registrationId)
        {
            Client.DeleteRegistration(registrationId);
        }

        public void PushNotification(Notification notification, Dictionary<string, string> tags)
        {
            Client.PushNotification(notification, tags);
        }

        public void PushSilentNotification(object data, Dictionary<string, string> tags)
        {
            Client.PushSilentNotification(data, tags);
        }
    }
}
