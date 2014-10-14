using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.Framework.SharedModel;
using Lunggo.Framework.TicketSupport.ZendeskClass;

namespace Lunggo.Framework.TicketSupport
{
    public class TicketSupportService
    {
        private ITicketSupportClient _Client;
        private static TicketSupportService instance = new TicketSupportService();


        private TicketSupportService()
        {

        }

        public void Init(ITicketSupportClient ticketClient)
        {
            _Client = ticketClient;
        }

        public static TicketSupportService GetInstance()
        {
            return instance;
        }
        public string CreateTicketAndReturnResponseStatus(BaseTicket TicketInClass)
        {
            try
            {
                return _Client.CreateTicketAndReturnResponseStatus(TicketInClass);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public string CreateTicketWithAttachmentAndReturnResponseStatus(BaseTicket TicketInClass, List<FileInfo> files)
        {
            try
            {
                return _Client.CreateTicketWithAttachmentAndReturnResponseStatus(TicketInClass, files);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


    }
}
