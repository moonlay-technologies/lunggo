using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Lunggo.ApCommon.Subscriber;
using Lunggo.Framework.Cors;
using Lunggo.Framework.Database;
using Lunggo.Framework.Extension;
using Lunggo.Framework.Mail;
using Lunggo.Framework.Queue;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;
using Lunggo.WebAPI.ApiSrc.v1.Newsletter.Model;
using Lunggo.WebAPI.ApiSrc.v1.Newsletter.Query;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Lunggo.WebAPI.ApiSrc.v1.Newsletter
{
    public class NewsletterController : ApiController
    {
        [LunggoCorsPolicy]
        [Route("api/v1/newsletter/subscribe")]
        [HttpPost]
        public bool NewsletterSubscribe(HttpRequestMessage httpRequest, [FromBody] NewsletterSubscribeInput input)
        {
            input.Address = input.Address.ToLower();
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var found = CheckEmailQuery.GetInstance().Execute(conn, new {Email = input.Address}).Single();
                if (Convert.ToBoolean(found))
                {
                    return false;
                }
                else
                {
                    var mailService = MailService.GetInstance();
                    var mailModel = new MailModel
                    {
                        RecipientList = new[] { "travorama.newsletter@gmail.com" },
                        FromMail = "newsletter@travorama.com",
                        FromName = "Newsletter Travorama",
                        Subject = input.Address
                    };
                    mailService.SendEmail(input.Address, mailModel, "Newsletter");
                    var queue = QueueService.GetInstance().GetQueueByReference("InitialSubscriberEmail");
                    var message = new SubscriberEmailModel
                    {
                        Email = input.Address
                    };
                    queue.AddMessage(new CloudQueueMessage(message.Serialize()));
                    NewsletterSubscriberTableRepo.GetInstance()
                        .Insert(conn, new NewsletterSubscriberTableRecord {Email = input.Address});
                    return true;
                }
            }
        }
    }
}
