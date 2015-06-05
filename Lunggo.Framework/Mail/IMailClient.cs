using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.Framework.Mail
{
    public interface IMailClient
    {
        void Init();

        void SendEmail<T>(T objectParam, MailModel mailModel, string partitionKey);
    }
}
