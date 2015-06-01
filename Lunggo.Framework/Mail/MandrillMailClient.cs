using Lunggo.Framework.Core;
using Lunggo.Framework.SharedModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mandrill;
using Mandrill.Models;
using Mandrill.Requests.Messages;

namespace Lunggo.Framework.Mail
{
    public class MandrillMailClient : IMailClient
    {
        MandrillApi apiOfMandrill;
        string _mandrillTemplate;
        bool enablingOtherEmailAddressVisibilityForRecipient = true;
        IMailTemplateEngine mailTemplateEngine;
        enum RecipientTypeEnum
        {
            to,
            cc,
            bcc
        }
        public void init(string mandrillAPIKey)
        {
            init(mandrillAPIKey, "MyTemplate", new RazorMailTemplateEngine());
        }
        public void init(string mandrillAPIKey, string mandrillTemplate, IMailTemplateEngine _mailTemplateEngine)
        {
            this.apiOfMandrill = new MandrillApi(mandrillAPIKey, true);
            _mandrillTemplate = mandrillTemplate;
            mailTemplateEngine = _mailTemplateEngine;
        }
        public void sendEmail<T>(T objectParam, MailModel mailModel, string partitionKey)
        {
            try
            {
                EmailMessage emailMessage = GenerateMessage(objectParam, mailModel, partitionKey);
                var emailMessageRequest = new SendMessageRequest(emailMessage);
                var returnvalue = apiOfMandrill.SendMessage(emailMessageRequest);
            }
            catch (Exception ex)
            {
                LunggoLogger.Error(ex.Message, ex);
                throw;
            }
        }
        private EmailMessage GenerateMessage<T>(T objectParam, MailModel mailModel, string partitionKey)
        {
            EmailMessage emailMessage = new EmailMessage();
            emailMessage.PreserveRecipients = this.enablingOtherEmailAddressVisibilityForRecipient;
            emailMessage.Subject = mailModel.Subject;
            emailMessage.FromEmail = mailModel.From_Mail;
            emailMessage.FromName = mailModel.From_Name;
            emailMessage.To = GenerateMessageAddressTo(mailModel);
            emailMessage.Html = this.mailTemplateEngine.GetEmailTemplate(objectParam, partitionKey);
            if (mailModel.ListFileInfo != null && mailModel.ListFileInfo.Count>0)
                emailMessage.Attachments = convertFileInfoToAttachmentFiles(mailModel.ListFileInfo);
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
        
        private IEnumerable<EmailAttachment> convertFileInfoToAttachmentFiles(List<FileInfo> files)
        {
            if (files == null || files.Count < 1)
            {
                yield break;
            }
            foreach (FileInfo file in files)
            {
                var base64OfAttachmentFile = Convert.ToBase64String(file.FileData, 0, file.FileData.Length);
                EmailAttachment attachmentToSend = new EmailAttachment
                {
                    Name = file.FileName,
                    Type  = file.ContentType,
                    Content = base64OfAttachmentFile
                };
                yield return attachmentToSend;
            }

        }

    }
}
