using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.Framework.Mail
{
    public interface IMailTemplateEngine
    {
        void Init(string mailTableName, string mailRowKey);
        string GetEmailTemplate<T>(T objectParam, string partitionKey);
    }
}
