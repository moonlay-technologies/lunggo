using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Service;
using Lunggo.Framework.Config;
using Lunggo.Framework.Database;
using Lunggo.Framework.HtmlTemplate;
using Lunggo.Framework.Mail;
using Lunggo.Framework.SharedModel;
using Microsoft.Azure.WebJobs;
using FileInfo = Lunggo.Framework.SharedModel.FileInfo;

namespace Lunggo.WebJob.EticketQueueHandler
{
    public class Functions
    {

        public static void ProcessQueueMessage([QueueTrigger("eticketQueue")] string message)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                if (message.First() == 'F')
                {
                    var flightService = FlightService.GetInstance();
                    var templateService = HtmlTemplateService.GetInstance();
                    var converter = new EvoPdf.HtmlToPdfConverter();
                    var summary = flightService.GetDetails(message);
                    var template = templateService.GenerateTemplate(summary, HtmlTemplateType.FlightEticket);
                    var fileContent = converter.ConvertHtml(template, null);
                    var mailTemplate = new MailDetailForQueue
                    {
                        FromMail = "saya@lagi.lho",
                        FromName = "Saya",
                        Subject = "Masih Ingat Saya, Kan?",
                        RecipientList = new[] {"developer@travelmadezy.com"},
                        ListFileInfo = new List<FileInfo>
                        {
                            new FileInfo
                            {
                                ContentType = "pdf",
                                FileName = "dari saya.pdf",
                                FileData = fileContent
                            }
                        },
                        MailTemplate = HtmlTemplateType.FlightEticket
                    };
                    var mailService = MailService.GetInstance();
                    mailService.SendEmail(summary, mailTemplate, HtmlTemplateType.FlightEticket);
                    // TODO Flight : get Html, Convert into PDF, Push Queue for Email
                }
            }
        }
    }
}
