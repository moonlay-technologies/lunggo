using System;
using System.Collections.Generic;
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
using FileInfo = Lunggo.Framework.SharedModel.FileInfo;

namespace Lunggo.WebJob.EmailQueueHandler
{
    public class Function
    {
        public static void TestSend()
        {
            var evo = new EvoPdf.HtmlToPdfConverter();
            MailService.GetInstance().SendEmail(new {Name = "yey", Address = "ow"}, new MailModel
            {
                Subject = "makan nih email",
                FromMail = "dari@saya.lho",
                FromName = "saya siapa",
                RecipientList = new []{"developer@travelmadezy.com"},
                ListFileInfo = new List<FileInfo>
                {
                    new FileInfo
                    {
                        ContentType = "pdf",
                        FileName = "apaini.pdf",
                        FileData = evo.ConvertUrl("http://www.google.com/")
                    }
                }
            }, "TestHtml");
        }

        public static void EmailQueueHandler([QueueTrigger("emailqueue")] MailDetailForQueue mailDetailInQueue)
        {

        }
    }
}
