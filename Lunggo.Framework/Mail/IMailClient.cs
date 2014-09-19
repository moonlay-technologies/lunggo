using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.Framework.Mail
{
    public interface IMailClient
    {
        void init(string apiKey);

        void sendEmail<T>(T objectParam, MailModel mailModel, string partitionKey);
    }
}
