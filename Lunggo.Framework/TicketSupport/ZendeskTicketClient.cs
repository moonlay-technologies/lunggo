using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.Framework.Config;
using Lunggo.Framework.Core;
using Lunggo.Framework.SharedModel;
using Lunggo.Framework.TicketSupport.ZendeskClass;
using RestSharp;
using FileInfo = Lunggo.Framework.SharedModel.FileInfo;

namespace Lunggo.Framework.TicketSupport
{
    public class ZendeskTicketClient : ITicketSupportClient
    {
        private ZendeskApi api;
        public void init(string apikey)
        {
            string zendeskSiteUrl = ConfigManager.GetInstance().GetConfigValue("zendesk", "zendeskSiteUrl");
            string zendeskEmailAccount = ConfigManager.GetInstance().GetConfigValue("zendesk", "zendeskEmailAccount");
            init(apikey, zendeskSiteUrl, zendeskEmailAccount);
        }
        public void init(string apikey, string zendeskSiteUrl, string zendeskEmailAccount)
        {
            api = new ZendeskApi(zendeskSiteUrl, zendeskEmailAccount, "", apikey);
        }
        public string CreateTicketAndReturnResponseStatus(BaseTicket baseTicket)
        {
            try
            {
                ZendeskTicket ticketInClass = (ZendeskTicket)baseTicket;
                var res = api.Tickets.CreateTicket(ticketInClass);
                return res.Audit.TicketId;
            }
            catch (Exception ex)
            {
                LunggoLogger.Error(ex.Message, ex);
                throw;
            }
        }

        public string CreateTicketWithAttachmentAndReturnResponseStatus(BaseTicket baseTicket,
            List<FileInfo> files)
        {
            try
            {
                ZendeskTicket ticketInClass = (ZendeskTicket)baseTicket;
                var attachment = api.Attachments.UploadAttachments(files);
                ticketInClass.Comment.Uploads = new List<string>() { attachment.Token };
                var res = api.Tickets.CreateTicket(ticketInClass);
                return res.Audit.TicketId;
            }
            catch (Exception ex)
            {
                LunggoLogger.Error(ex.Message, ex);
                throw;
            }
        }

    }
}
