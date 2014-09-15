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
        private IZendeskClient _Client;
        private static TicketSupportService instance = new TicketSupportService();


        private TicketSupportService()
        {

        }

        public void Init(string apikey)
        {
            _Client = new ZendeskClient();
            _Client.init(apikey);
        }

        public static TicketSupportService GetInstance()
        {
            return instance;
        }
        public string CreateTicketAndReturnResponseStatus(ZendeskTicket TicketInClass)
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
        public string CreateTicketWithAttachmentAndReturnResponseStatus(ZendeskTicket TicketInClass, List<FileInfo> files)
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
