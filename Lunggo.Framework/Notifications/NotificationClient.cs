using System.Collections.Generic;

namespace Lunggo.Framework.Notifications
{
    internal abstract class NotificationClient
    {
        internal abstract void Init(string connString, string hubName);
        internal abstract string RegisterDevice(string notificationHandle, string deviceId);
        internal abstract bool UpdateTags(string registrationId, string notificationHandle, Platform platform,
            Dictionary<string, string> newTags);
        internal abstract void DeleteRegistration(string registrationId);
        internal abstract void PushNotification(Notification notification, Dictionary<string, string> tags);
        internal abstract void PushSilentNotification(object data, Dictionary<string, string> tags);
    }
}
