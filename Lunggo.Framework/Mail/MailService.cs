using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.Framework.Mail
{
    public class MailService
    {
        IMailClient _mailClient;
        private static readonly MailService Instance = new MailService();


        private MailService()
        {
            
        }
        public void Init(IMailClient client)
        {
            _mailClient = client;
        }
        public static MailService GetInstance()
        {
            return Instance;
        }
        public void SendEmail<T>(T objectParam, MailModel mailModel, string partitionKey)
        {
            _mailClient.sendEmail(objectParam, mailModel, partitionKey);
        }
    }
}
