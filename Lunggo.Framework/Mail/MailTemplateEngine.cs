using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.Framework.Mail
{
    internal abstract class MailTemplateEngine
    {
        internal abstract void Init(string mailTableName, string mailRowKey);
        internal abstract string GetEmailTemplate<T>(T objectParam, string partitionKey);
    }
}
