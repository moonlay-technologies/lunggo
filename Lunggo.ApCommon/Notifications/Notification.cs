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
        public NotificationDataJsonFormat Data { get; set; }
    }

    public class NotificationJsonFormat
    {
        public string to { get; set; }
        public string title { get; set; }
        public string body { get; set; }
        public string sound { get; set; }
        public NotificationDataJsonFormat data { get; set; }
    }

    public class NotificationData
    {
        public string Function { get; set; }
        public string Status { get; set; }
    }

    public class NotificationDataJsonFormat
    {
        public string function { get; set; }
        public string status { get; set; }
    }
}
