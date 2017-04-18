using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.Framework.Log
{
    internal abstract class LogClient
    {
        internal abstract void Init(string webhookUrl);
        internal abstract void Post(string text);
        internal abstract void Post(string text, string recipient);
        internal abstract void Post(string text, string recipient, List<LogAttachment> attachments);
    }
}
