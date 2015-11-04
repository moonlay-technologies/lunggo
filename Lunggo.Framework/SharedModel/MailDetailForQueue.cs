using Lunggo.Framework.Mail;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;

namespace Lunggo.Framework.SharedModel
{
    public class MailDetailForQueue : MailModel
    {
        public object MailObjectDetail { get; set; }
        public string MailTemplate { get; set; }
        public CloudQueueMessage SerializeToQueueMessage()
        {
            string classInJson = JsonConvert.SerializeObject(this);
            return new CloudQueueMessage(classInJson);
        }
    }
}
