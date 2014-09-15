using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Razor.Generator;
using Lunggo.Framework.Config;
using Lunggo.Framework.SharedModel;
using Newtonsoft.Json;
using RestSharp;
using FileInfo = Lunggo.Framework.SharedModel.FileInfo;

namespace Lunggo.Framework.TicketSupport
{
    public class FreshdeskClient : IFreshdeskClient
    {
        private RestClient restClientRestSharp;
        private HttpBasicAuthenticator Authorization;

        private enum HeaderContentType
        {
            ApplicationJson,
            MultipartFormData
        }

        public void init(string apikey)
        {
            string freshdeskRestSharpClientUrl = ConfigManager.GetInstance().GetConfigValue("freshdesk", "RestSharpClientUrl");
            this.restClientRestSharp = new RestClient(freshdeskRestSharpClientUrl);
            //this.restClientRestSharp.Authenticator = new HttpBasicAuthenticator(apikey, "X");
        }

        private RestRequest GenerateRestRequest(string restSharpRequestUrl, Method MethodType, HeaderContentType contentType)
        {
            try
            {
                var requestRestSharp = new RestRequest(restSharpRequestUrl, MethodType);
                if (contentType.Equals(HeaderContentType.ApplicationJson))
                {
                    GenerateRestRequestHeaderForApplicationJson(requestRestSharp);
                }
                else if (contentType.Equals(HeaderContentType.MultipartFormData))
                {

                }
                return requestRestSharp;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void GenerateRestRequestHeaderForApplicationJson(RestRequest requestRestSharp)
        {
            requestRestSharp.AddHeader("Authorization", "Basic dHVmSVFPNERnT1pTVGZBc2JDV3U6WA==");
            requestRestSharp.AddHeader("Accept", "application/json");
            requestRestSharp.AddHeader("Content-Type", "application/json");
            requestRestSharp.XmlSerializer.ContentType = "application/json";
            requestRestSharp.RequestFormat = DataFormat.Json;
        }
        private void GenerateRestRequestHeaderForMultipartFormData(RestRequest requestRestSharp)
        {
            //requestRestSharp.AddHeader("Authorization", this.Authorization);
            requestRestSharp.AddHeader("Content-Type", "multipart/form-data");
            requestRestSharp.RequestFormat = DataFormat.Json;
        }

        public string CreateTicketAndReturnResponseStatus(FreshdeskTicketJson TicketInClass)
        {
            try
            {
                string restSharpRequestUrl = "/helpdesk/tickets.json";
                string TicketInJson = JsonConvert.SerializeObject(TicketInClass);

                string jso = @"{
      ""helpdesk_ticket"":{
      ""description_html"":""asdddddddddddddddddd asdddddddddddddddddddddd ddddddddddddddddddddddddddddddddddddddddd"",
      ""subject"":""Coba 2"",
      ""email"":""bayualvian@hotmail.com"",
      ""priority"":1,
      ""status"":2
  }

      ""responder_id"":5000024759
}";
                var requestRestSharp = GenerateRestRequest(restSharpRequestUrl, Method.POST, HeaderContentType.ApplicationJson);
                requestRestSharp.AddParameter("application/json", jso, ParameterType.RequestBody);
                var responses = restClientRestSharp.Execute(requestRestSharp);
                return responses.Content;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public string CreateTicketWithAttachmentAndReturnResponseStatus(FreshdeskTicketJson TicketInClass, List<FileInfo> files)
        {
            try
            {
                string restSharpRequestUrl = "/helpdesk/tickets.json";
                string prefixFileName = "/WebApiAttachment/";
                var requestRestSharp = GenerateRestRequest(restSharpRequestUrl, Method.POST, HeaderContentType.MultipartFormData);


                requestRestSharp.AddParameter("helpdesk_ticket[email]", TicketInClass.email);
                requestRestSharp.AddParameter("helpdesk_ticket[requester_id]", TicketInClass.requester_id);
                requestRestSharp.AddParameter("helpdesk_ticket[subject]", TicketInClass.subject);
                requestRestSharp.AddParameter("helpdesk_ticket[description]", TicketInClass.description);
                requestRestSharp.AddParameter("helpdesk_ticket[description_html]", TicketInClass.description_html);
                requestRestSharp.AddParameter("helpdesk_ticket[status]", TicketInClass.status);
                requestRestSharp.AddParameter("helpdesk_ticket[priority]", TicketInClass.priority);
                requestRestSharp.AddParameter("helpdesk_ticket[source]", TicketInClass.source);
                requestRestSharp.AddParameter("helpdesk_ticket[deleted]", TicketInClass.deleted);
                requestRestSharp.AddParameter("helpdesk_ticket[spam]", TicketInClass.spam);
                requestRestSharp.AddParameter("helpdesk_ticket[responder_id]", TicketInClass.responder_id);
                requestRestSharp.AddParameter("helpdesk_ticket[group_id]", TicketInClass.group_id);
                requestRestSharp.AddParameter("helpdesk_ticket[ticket_type]", TicketInClass.ticket_type);
                foreach (string email in TicketInClass.to_email)
                {
                    requestRestSharp.AddParameter("helpdesk_ticket[to_email][]", email);
                }
                foreach (string email in TicketInClass.cc_email)
                {
                    requestRestSharp.AddParameter("helpdesk_ticket[cc_email][]", email);
                }
                requestRestSharp.AddParameter("helpdesk_ticket[email_config_id]", TicketInClass.email_config_id);
                requestRestSharp.AddParameter("helpdesk_ticket[isescalated]", TicketInClass.isescalated);
                requestRestSharp.AddParameter("helpdesk_ticket[due_by]", TicketInClass.due_by);
                foreach (var attachment in files)
                {
                    requestRestSharp.AddFile("helpdesk_ticket[attachments][]", attachment.FileData, prefixFileName + attachment.FileName, attachment.ContentType);
                }
                var responses = restClientRestSharp.Execute(requestRestSharp);

                
                return responses.Content;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
