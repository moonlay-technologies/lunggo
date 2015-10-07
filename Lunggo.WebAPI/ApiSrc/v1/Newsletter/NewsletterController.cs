using System.Linq;
using System.Web.Http;
using Lunggo.ApCommon.Subscriber;
using Lunggo.Framework.Cors;
using Lunggo.Framework.Database;
using Lunggo.Framework.Extension;
using Lunggo.Framework.HtmlTemplate;
using Lunggo.Framework.Mail;
using Lunggo.Framework.Queue;
using Lunggo.WebAPI.ApiSrc.v1.Newsletter.Query;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Lunggo.WebAPI.ApiSrc.v1.Newsletter
{
    public class NewsletterController : ApiController
    {
        [HttpGet]
        [LunggoCorsPolicy]
        [Route("api/v1/newsletter/subscribe")]
        [HttpPost]
        public bool NewsletterSubscribe(string address, string name)
        {
            using (var conn = DbService.GetInstance().GetOpenConnection())
            {
                var found = CheckEmailQuery.GetInstance().Execute(conn, new {Email = address}).Single();
                if (found)
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
                        Subject = address
                    };
                    mailService.SendEmail(address, mailModel, HtmlTemplateType.Newsletter);
                    var queue = QueueService.GetInstance().GetQueueByReference(Queue.InitialSubscriberEmail);
                    var message = new SubscriberEmailModel
                    {
                        Email = address
                    };
                    queue.AddMessage(new CloudQueueMessage(message.Serialize()));
                    return true;
                }
            }
        }
    }
}
