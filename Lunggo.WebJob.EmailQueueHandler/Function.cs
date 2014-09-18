using System;
using System.IO;
//using Lunggo.Framework.Mail;
//using Lunggo.Framework.SharedModel;
//using Lunggo.Framework.TicketSupport;
//using Lunggo.Framework.TicketSupport.ZendeskClass;
//using Microsoft.Azure.WebJobs;
//using Microsoft.WindowsAzure.Storage.Queue;
//using Newtonsoft.Json.Linq;
//using ZendeskApi_v2.Models.Constants;
//using Lunggo.Framework.Queue;
//using Lunggo.Framework.Util;
using Lunggo.Framework.Mail;
using Lunggo.Framework.SharedModel;
using Lunggo.Framework.Util;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json.Linq;

namespace Lunggo.WebJob.EmailQueueHandler
{
    public class Function
    {
        public static void EmailQueueHandler([QueueTrigger("emailqueue")] MailDetailForQueue mailDetailInQueue)
        {
            switch (mailDetailInQueue.MailTemplate)
            {
                case MailTemplateEnum.SuccessBooking:
                    SuccessBooking(mailDetailInQueue);
                    break;
                case MailTemplateEnum.ApalagiGitu:
                    break;
                case MailTemplateEnum.TestHtml:
                    TestHtml(mailDetailInQueue);
                    break;
            }

        }
        public static void SuccessBooking(MailDetailForQueue mailDetail)
        {
            try
            {
                MailService.GetInstance().sendEmail((mailDetail.MailObjectDetail as JObject).ToObject<BookingDetail>(), mailDetail, mailDetail.MailTemplate.convertToString());
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static void TestHtml(MailDetailForQueue mailDetail)
        {
            try
            {
                MailService.GetInstance().sendEmail((mailDetail.MailObjectDetail as JObject).ToObject<BookingDetail>(), mailDetail, mailDetail.MailTemplate.convertToString());
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
