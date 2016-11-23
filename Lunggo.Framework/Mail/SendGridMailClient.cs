using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System;
using Mandrill;
using System.Web.Script.Serialization;
using SendGrid;


namespace Lunggo.Framework.Mail
{
    public partial class MailService
    {
        public class SendGridMailClient
        {
            private static readonly SendGridMailClient ClientInstance = new SendGridMailClient();
            private SendGrid.Web _apiOfSendGrid = new SendGrid.Web("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");
            private enum RecipientType
            {
                To,
                Cc,
                Bcc
            }
            private SendGridMailClient()
            {
            }
            internal static SendGridMailClient GetInstance()
            {
                return ClientInstance;
            }
            internal void SendEmail<T>(T objectParam, MailModel mailModel, string template)
            {
                var emailMessage = GenerateMessage(objectParam, mailModel, template);
                _apiOfSendGrid.DeliverAsync(emailMessage);
            }

            private SendGridMessage GenerateMessage<T>(T objectParam, MailModel mailModel, string template)
            {
                var sendGridMessage = new SendGridMessage
                {
                    Subject = mailModel.Subject,
                    From = new MailAddress(mailModel.FromMail, mailModel.FromName),
                    To = GenerateMessageAddressTo(mailModel),
                    Html = HtmlTemplate.HtmlTemplateService.GetInstance().GenerateTemplate(objectParam, template)
                };
                return sendGridMessage;
            }
            private MailAddress[] GenerateMessageAddressTo(MailModel mailModel)
            {
                var addresses = new List<MailAddress>();
                if (mailModel.RecipientList != null)
                    addresses.AddRange(GenerateRecipients(mailModel.RecipientList));
                if (mailModel.CcList != null)
                    addresses.AddRange(GenerateRecipients(mailModel.CcList));
                if (mailModel.BccList != null)
                    addresses.AddRange(GenerateRecipients(mailModel.BccList));
                return addresses.ToArray();
            }
            private static IEnumerable<MailAddress> GenerateRecipients(IEnumerable<String> listAddress)
            {
                return listAddress.Select(address => new MailAddress(address));
            }
        }
    }
}
