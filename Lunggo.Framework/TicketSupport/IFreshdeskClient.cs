using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.Framework.SharedModel;

namespace Lunggo.Framework.TicketSupport
{
    public interface IFreshdeskClient
    {

        void init(string apikey);
        string CreateTicketAndReturnResponseStatus(FreshdeskTicketJson TicketInClass);
        string CreateTicketWithAttachmentAndReturnResponseStatus(FreshdeskTicketJson TicketInClass, List<FileInfo> files);
    }
}
