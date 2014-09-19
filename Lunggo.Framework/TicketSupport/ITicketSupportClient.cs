using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.Framework.SharedModel;
using Lunggo.Framework.TicketSupport.ZendeskClass;

namespace Lunggo.Framework.TicketSupport
{
    public interface ITicketSupportClient
    {

        void init(string apikey);
        string CreateTicketAndReturnResponseStatus(BaseTicket TicketInClass);
        string CreateTicketWithAttachmentAndReturnResponseStatus(BaseTicket TicketInClass, List<FileInfo> files);
    }
}
