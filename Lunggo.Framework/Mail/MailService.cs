using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.Framework.Mail
{
    public partial class MailService
    {
        private static readonly MailService Instance = new MailService();
        private bool _isInitialized;
        private static readonly MandrillMailClient Client = MandrillMailClient.GetClientInstance();

        private MailService()
        {
            
        }

        public void Init()
        {
            if (!_isInitialized)
            {
                Client.Init();
                _isInitialized = true;
            }
            else
            {
                throw new InvalidOperationException("MailService is already initialized");
            }
        }

        public static MailService GetInstance()
        {
            return Instance;
        }

        public void SendEmail<T>(T objectParam, MailModel mailModel, string partitionKey)
        {
            Client.SendEmail(objectParam, mailModel, partitionKey);
        }
    }
}
