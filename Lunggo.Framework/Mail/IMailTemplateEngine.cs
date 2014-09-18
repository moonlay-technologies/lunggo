using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.Framework.Mail
{
    public interface IMailTemplateEngine
    {
        void init(string defaultMailTable, string defaultRowKey);
        string GetEmailTemplate<T>(T objectParam, string partitionKey);
    }
}
