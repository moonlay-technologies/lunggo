using System.Collections.Generic;
using System.Drawing;

namespace Lunggo.Framework.Notification
{
    internal abstract class NotificationClient
    {
        internal abstract void Init(string connString, string hubName);
        internal abstract string RegisterDevice(string notificationHandle, string deviceId);
        internal abstract bool SetTags(string registrationId, string notificationHandle, Platform platform,
            IEnumerable<string> tags);
        internal abstract bool AddTags(string registrationId, string notificationHandle, Platform platform,
            IEnumerable<string> tags);
        internal abstract void DeleteRegistration(string registrationId);

    }
}
