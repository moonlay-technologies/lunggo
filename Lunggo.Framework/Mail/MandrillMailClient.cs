using Lunggo.Framework.Config;
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
    public partial class MailService {
        private class MandrillMailClient : MailClient
        {
            private static readonly MandrillMailClient ClientInstance = new MandrillMailClient();
            private bool _isInitialized;
            private MandrillApi _apiOfMandrill;
            private bool _exposeRecipients;
            private MailTemplateEngine _mailTemplateEngine;

            private enum RecipientType
            {
                To,
                Cc,
                Bcc
            }

            private MandrillMailClient()
            {
                
            }

            internal static MandrillMailClient GetClientInstance()
            {
                return ClientInstance;
            }

            internal override void Init()
            {
                if (!_isInitialized)
                {
                    var mandrillApiKey = ConfigManager.GetInstance().GetConfigValue("mandrill", "apiKey");
                    _apiOfMandrill = new MandrillApi(mandrillApiKey);
                    var razorMailTemplateEngine = new RazorMailTemplateEngine();
                    var mailTable = ConfigManager.GetInstance().GetConfigValue("mandrill", "mailTableName");
                    var rowKey = ConfigManager.GetInstance().GetConfigValue("mandrill", "mailRowName");
                    razorMailTemplateEngine.Init(mailTable, rowKey);
                    _mailTemplateEngine = razorMailTemplateEngine;
                    _exposeRecipients = bool.Parse(ConfigManager.GetInstance().GetConfigValue("mandrill", "exposeRecipients"));
                    _isInitialized = true;
                }
                else
                {
                    throw new InvalidOperationException("MandrillMailClient is already initialized");
                }
            }

            internal override void SendEmail<T>(T objectParam, MailModel mailModel, string partitionKey)
            {
                try
                {
                    EmailMessage emailMessage = GenerateMessage(objectParam, mailModel, partitionKey);
                    var emailMessageRequest = new SendMessageRequest(emailMessage);
                    var returnvalue = _apiOfMandrill.SendMessage(emailMessageRequest);
                }
                catch (Exception ex)
                {
                    LunggoLogger.Error(ex.Message, ex);
                    throw;
                }
            }

            private EmailMessage GenerateMessage<T>(T objectParam, MailModel mailModel, string partitionKey)
            {
                var emailMessage = new EmailMessage
                {
                    PreserveRecipients = !_exposeRecipients,
                    Subject = mailModel.Subject,
                    FromEmail = mailModel.FromMail,
                    FromName = mailModel.FromName,
                    To = GenerateMessageAddressTo(mailModel),
                    Html = _mailTemplateEngine.GetEmailTemplate(objectParam, partitionKey)
                };
                if (mailModel.ListFileInfo != null && mailModel.ListFileInfo.Count > 0)
                    emailMessage.Attachments = ConvertFileInfoToAttachmentFiles(mailModel.ListFileInfo);
                return emailMessage;
            }

            private IEnumerable<EmailAddress> GenerateMessageAddressTo(MailModel mailModel)
            {
                var addresses = new List<EmailAddress>();
                if (mailModel.RecipientList != null)
                    addresses.AddRange(GenerateRecipients(mailModel.RecipientList, RecipientType.To));
                if (mailModel.CcList != null)
                    addresses.AddRange(GenerateRecipients(mailModel.CcList, RecipientType.Cc));
                if (mailModel.BccList != null)
                    addresses.AddRange(GenerateRecipients(mailModel.BccList, RecipientType.Bcc));
                return addresses;

            }

            private static IEnumerable<EmailAddress> GenerateRecipients(IEnumerable<string> listAddress, RecipientType sendingType)
            {
                return listAddress.Select(address => new EmailAddress(address, address, sendingType.ToString()));
            }

            private static IEnumerable<EmailAttachment> ConvertFileInfoToAttachmentFiles(List<FileInfo> files)
            {
                if (files == null || files.Count < 1)
                {
                    yield break;
                }
                foreach (FileInfo file in files)
                {
                    var base64OfAttachmentFile = Convert.ToBase64String(file.FileData, 0, file.FileData.Length);
                    var attachmentToSend = new EmailAttachment
                    {
                        Name = file.FileName,
                        Type = file.ContentType,
                        Content = base64OfAttachmentFile
                    };
                    yield return attachmentToSend;
                }

            }
        }

    }
}
