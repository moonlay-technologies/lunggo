using Lunggo.Framework.SharedModel;
using Mandrill;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.Framework.Mail
{
    public class MandrillMailClient : IMailClient
    {
        MandrillApi apiOfMandrill;
        string _defaultMandrillTemplate = "MyTemplate";
        public void init(string mandrillAPIKey)
        {
            this.apiOfMandrill = new MandrillApi(mandrillAPIKey, true);
        }
        public void sendEmail<T>(T objectParam, MailModel mailModel, string partitionKey)
        {
            try
            {
                EmailMessage emailMessage = GenerateMessage(objectParam, mailModel, partitionKey);
                var returnvalue = apiOfMandrill.SendMessage(emailMessage, this._defaultMandrillTemplate, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private EmailMessage GenerateMessage<T>(T objectParam, MailModel mailModel, string partitionKey)
        {
            IEnumerable<attachment> attachmentsToSend = convertHTTPFilesToAttachmentFiles(mailModel.ListFileInfo);
            IMailTemplateEngine templateEngine = new MailTemplateEngine();
            EmailMessage emailMessage = new EmailMessage();
            emailMessage.preserve_recipients = true;
            emailMessage.html = templateEngine.GetEmailTemplate(objectParam, partitionKey);
            emailMessage.to = GenerateMessageAddressTo(mailModel);
            emailMessage.attachments = attachmentsToSend;
            return emailMessage;
        }
        private IEnumerable<EmailAddress> GenerateMessageAddressTo(MailModel mailModel)
        {
            List<EmailAddress> addresses = new List<EmailAddress>();
            addresses.AddRange(GenerateAddressTo(mailModel.ListRecepient));
            addresses.AddRange(GenerateAddressCC(mailModel.ListCC));
            addresses.AddRange(GenerateAddressBCC(mailModel.ListBCC));
            return addresses;

        }
        private List<EmailAddress> GenerateAddressTo(List<string> listTo)
        {
            return GenerateRecepient(listTo, "to");
        }
        private List<EmailAddress> GenerateAddressCC(List<string> listCC)
        {
            return GenerateRecepient(listCC, "cc");
        }
        private List<EmailAddress> GenerateAddressBCC(List<string> listBCC)
        {
            return GenerateRecepient(listBCC, "bcc");
        }
        private List<EmailAddress> GenerateRecepient(List<string> listAddress, string sendingType)
        {
            List<EmailAddress> addresses = new List<EmailAddress>();
            foreach (string address in listAddress)
            {
                addresses.Add( new EmailAddress(address, address, sendingType));
            }
            return addresses;
        }
        private IEnumerable<attachment> convertHTTPFilesToAttachmentFiles(List<FileInfo> files)
        {
            foreach (FileInfo file in files)
            {
                var base64OfAttachmentFile = Convert.ToBase64String(file.ArrayData, 0, file.ArrayData.Length);
                attachment attachmentToSend = new attachment
                {
                    name = file.Name,
                    type = file.ContentType,
                    content = base64OfAttachmentFile
                };
                yield return attachmentToSend;
            }

        }

    }
}
