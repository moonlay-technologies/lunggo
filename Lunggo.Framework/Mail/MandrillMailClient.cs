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
        string _defaultFromName = "Lunggo System";
        string _defaultFromMail = "bayu.alvian@moonlay.com";
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
            IEnumerable<attachment> attachmentsToSend = convertFileInfoToAttachmentFiles(mailModel.ListFileInfo);
            EmailMessage emailMessage = new EmailMessage();
            emailMessage.preserve_recipients = true;
            emailMessage.subject = mailModel.Subject;
            emailMessage.from_email = mailModel.From_Mail;
            emailMessage.from_name = mailModel.From_Name;
            emailMessage.html = new MailTemplateEngine().GetEmailTemplate(objectParam, partitionKey);
            emailMessage.to = GenerateMessageAddressTo(mailModel);
            if (attachmentsToSend!=null)
                emailMessage.attachments = attachmentsToSend;
            return emailMessage;
        }
        private IEnumerable<EmailAddress> GenerateMessageAddressTo(MailModel mailModel)
        {
            List<EmailAddress> addresses = new List<EmailAddress>();
            addresses.AddRange(GenerateAddressTo(mailModel.RecipientList));
            addresses.AddRange(GenerateAddressCC(mailModel.CCList));
            addresses.AddRange(GenerateAddressBCC(mailModel.BCCList));
            return addresses;

        }
        private List<EmailAddress> GenerateAddressTo(List<string> RecipientList)
        {
            return GenerateRecepient(RecipientList, "to");
        }
        private List<EmailAddress> GenerateAddressCC(List<string> CCList)
        {
            return GenerateRecepient(CCList, "cc");
        }
        private List<EmailAddress> GenerateAddressBCC(List<string> BCCList)
        {
            return GenerateRecepient(BCCList, "bcc");
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
        private IEnumerable<attachment> convertFileInfoToAttachmentFiles(List<FileInfo> files)
        {
            if (files == null || files.Count < 1)
            {
                yield break;
            }
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
