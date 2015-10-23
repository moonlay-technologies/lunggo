using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.Framework.HtmlTemplate;

namespace Lunggo.Framework.Mail
{
    internal abstract class MailClient
    {
        internal abstract void Init(string apiKey);
        internal abstract void SendEmail<T>(T objectParam, MailModel mailModel, string type);
    }
}
