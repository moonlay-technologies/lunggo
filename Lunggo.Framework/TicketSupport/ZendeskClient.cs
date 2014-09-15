using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.Framework.Config;
using Lunggo.Framework.SharedModel;
using Lunggo.Framework.TicketSupport.ZendeskClass;
using RestSharp;
using FileInfo = Lunggo.Framework.SharedModel.FileInfo;

namespace Lunggo.Framework.TicketSupport
{
    public class ZendeskClient : IZendeskClient
    {
        private ZendeskApi api;
        public void init(string apikey)
        {
            string zendeskSiteUrl = ConfigManager.GetInstance().GetConfigValue("zendesk", "zendeskSiteUrl");
            string zendeskEmailAccount = ConfigManager.GetInstance().GetConfigValue("zendesk", "zendeskEmailAccount");
            api = new ZendeskApi(zendeskSiteUrl, zendeskEmailAccount, "",apikey);
        }
        public string CreateTicketAndReturnResponseStatus(ZendeskTicket ticketInClass)
        {
            try
            {
                var res = api.Tickets.CreateTicket(ticketInClass);
                return res.Audit.TicketId;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string CreateTicketWithAttachmentAndReturnResponseStatus(ZendeskTicket ticketInClass,
            List<FileInfo> files)
        {
            try
            {
                var attachment = api.Attachments.UploadAttachments(files);
                ticketInClass.Comment.Uploads = new List<string>() { attachment.Token };
                var res = api.Tickets.CreateTicket(ticketInClass);
                return res.Audit.TicketId;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
