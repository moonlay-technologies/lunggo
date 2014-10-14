using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.Framework.Mail;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;

namespace Lunggo.Framework.SharedModel
{
    public class MailDetailForQueue : MailModel
    {
        public object MailObjectDetail { get; set; }
        public MailTemplateEnum MailTemplate { get; set; }
        public CloudQueueMessage SerializeToQueueMessage()
        {
            string classInJson = JsonConvert.SerializeObject(this);
            return new CloudQueueMessage(classInJson);
        }
    }
    public enum MailTemplateEnum
    {
        SuccessBooking,
        ApalagiGitu,
        TestHtml,
        TestHtmlWithAttachment
    }
}
