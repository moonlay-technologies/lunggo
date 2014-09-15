using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.Framework.SharedModel;
using Lunggo.Framework.TicketSupport.ZendeskClass;

namespace Lunggo.Framework.TicketSupport
{
    interface IZendeskClient
    {

        void init(string apikey);
        string CreateTicketAndReturnResponseStatus(ZendeskTicket TicketInClass);
        string CreateTicketWithAttachmentAndReturnResponseStatus(ZendeskTicket TicketInClass, List<FileInfo> files);
    }
}
