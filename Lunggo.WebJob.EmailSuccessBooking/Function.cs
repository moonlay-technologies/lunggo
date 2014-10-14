using System;
using System.IO;
using Lunggo.Framework.Mail;
using Lunggo.Framework.SharedModel;
using Lunggo.Framework.TicketSupport;
using Lunggo.Framework.TicketSupport.ZendeskClass;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json.Linq;
using ZendeskApi_v2.Models.Constants;
using Lunggo.Framework.Queue;
using Lunggo.Framework.Util;
namespace Lunggo.WebJob.EmailSuccessBooking
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

    }
}
