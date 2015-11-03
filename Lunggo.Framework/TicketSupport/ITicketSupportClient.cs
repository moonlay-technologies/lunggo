using System.Collections.Generic;
using Lunggo.Framework.SharedModel;

namespace Lunggo.Framework.TicketSupport
{
    public interface ITicketSupportClient
    {

        void init(string apikey);
        string CreateTicketAndReturnResponseStatus(BaseTicket TicketInClass);
        string CreateTicketWithAttachmentAndReturnResponseStatus(BaseTicket TicketInClass, List<FileInfo> files);
    }
}
