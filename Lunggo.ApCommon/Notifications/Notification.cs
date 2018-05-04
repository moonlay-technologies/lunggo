using System.Collections.Generic;
using System.Drawing;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Notifications
{
    public class Notification
    {
        public string Receiver { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string Sound { get; set; }
    }

    public class NotificationJsonFormat
    {
        public string to { get; set; }
        public string title { get; set; }
        public string body { get; set; }
        public string sound { get; set; }
    }
}
