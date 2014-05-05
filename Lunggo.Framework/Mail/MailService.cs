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
        private static MailService instance = new MailService();


        private MailService()
        {
            
        }
        public void init(IMailClient client)
        {
            _mailClient = client;
        }
        public static MailService GetInstance()
        {
            return instance;
        }
        public void sendEmail<T>(T objectParam, MailModel mailModel, string partitionKey)
        {
            _mailClient.sendEmail(objectParam, mailModel, partitionKey);
        }
    }
}
