using System.Collections.Generic;

namespace Lunggo.Framework.Notifications
{
    internal abstract class NotificationClient
    {
        internal abstract void Init(string connString, string hubName);
        internal abstract string RegisterDevice(string notificationHandle, string deviceId);
        internal abstract bool SetTags(string registrationId, string notificationHandle, Platform platform,
            Dictionary<string, string> tags);
        internal abstract bool AddTags(string registrationId, string notificationHandle, Platform platform,
            Dictionary<string, string> tags);
        internal abstract void DeleteRegistration(string registrationId);
        internal abstract void PushNotification(Dictionary<string, string> tags, Notification notification);
        internal abstract void PushSilentNotification(Dictionary<string, string> tags, object data);
    }
}
