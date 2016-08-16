using System.Collections.Generic;
using System.Drawing;

namespace Lunggo.Framework.Notifications
{
    public class Notification
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string Image { get; set; }
        public string Icon { get; set; }
        public object CustomData { get; set; }
    }
}
