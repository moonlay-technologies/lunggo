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
        bool enablingOtherEmailAddressVisibilityForRecipient = true;
        IMailTemplateEngine mailTemplateEngine = new RazorMailTemplateEngine();
        enum RecipientTypeEnum
        {
            to,
            cc,
            bcc
        }
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
            EmailMessage emailMessage = new EmailMessage();
            emailMessage.preserve_recipients = this.enablingOtherEmailAddressVisibilityForRecipient;
            emailMessage.subject = mailModel.Subject;
            emailMessage.from_email = mailModel.From_Mail;
            emailMessage.from_name = mailModel.From_Name;
            emailMessage.to = GenerateMessageAddressTo(mailModel);
            emailMessage.html = this.mailTemplateEngine.GetEmailTemplate(objectParam, partitionKey);
            if (mailModel.ListFileInfo != null && mailModel.ListFileInfo.Count>0)
                emailMessage.attachments = convertFileInfoToAttachmentFiles(mailModel.ListFileInfo);
            return emailMessage;
        }
        private IEnumerable<EmailAddress> GenerateMessageAddressTo(MailModel mailModel)
        {
            List<EmailAddress> addresses = new List<EmailAddress>();
            if (mailModel.RecipientList!=null)
                addresses.AddRange(GenerateAddressToByListString(mailModel.RecipientList));
            if (mailModel.CCList != null)
                addresses.AddRange(GenerateAddressCCByListString(mailModel.CCList));
            if (mailModel.BCCList != null)
                addresses.AddRange(GenerateAddressBCCByListString(mailModel.BCCList));
            return addresses;

        }
        private List<EmailAddress> GenerateAddressToByListString(string[] RecipientList)
        {
            return GenerateRecipientTypeByListString(RecipientList, RecipientTypeEnum.to.ToString());
        }
        private List<EmailAddress> GenerateAddressCCByListString(string[] CCList)
        {
            return GenerateRecipientTypeByListString(CCList, RecipientTypeEnum.cc.ToString());
        }
        private List<EmailAddress> GenerateAddressBCCByListString(string[] BCCList)
        {
            return GenerateRecipientTypeByListString(BCCList, RecipientTypeEnum.bcc.ToString());
        }
        private List<EmailAddress> GenerateRecipientTypeByListString(string[] listAddress, string sendingType)
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
