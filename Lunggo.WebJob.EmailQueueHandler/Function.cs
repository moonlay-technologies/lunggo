﻿using System;
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
using log4net;
using Lunggo.Framework.Core;
using Lunggo.Framework.Mail;
using Lunggo.Framework.SharedModel;
using Lunggo.Framework.Util;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json.Linq;

namespace Lunggo.WebJob.EmailQueueHandler
{
    public class Function
    {
        public static void TestSend()
        {
            MailService.GetInstance().SendEmail(new {Name = "yey", Address = "ow"}, new MailModel
            {
                Subject = "makan nih email",
                From_Mail = "dari@saya.lho",
                From_Name = "saya siapa",
                RecipientList = new []{"developer@travelmadezy.com"}
            }, "TestHtml");
        }

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
                case MailTemplateEnum.TestHtmlWithAttachment:
                    TestHtmlWithAttachment(mailDetailInQueue);
                    break;
            }
        }
        public static void SuccessBooking(MailDetailForQueue mailDetail)
        {
            try
            {
                string emailTemplateName = "SuccessBooking";
                MailService.GetInstance().SendEmail((mailDetail.MailObjectDetail as JObject).ToObject<BookingDetail>(), mailDetail, emailTemplateName);
            }
            catch (Exception)
            {
                LunggoLogger.Info("message");
                throw;
            }
        }
        public static void TestHtml(MailDetailForQueue mailDetail)
        {
            try
            {
                string emailTemplateName = "TestHtml";
                MailService.GetInstance().SendEmail((mailDetail.MailObjectDetail as JObject).ToObject<BookingDetail>(), mailDetail, emailTemplateName);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static void TestHtmlWithAttachment(MailDetailForQueue mailDetail)
        {
            try
            {
                string emailTemplateName = "TestHtml";
                MailService.GetInstance().SendEmail((mailDetail.MailObjectDetail as JObject).ToObject<BookingDetail>(), mailDetail, emailTemplateName);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
