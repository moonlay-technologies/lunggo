using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Lunggo.Framework.Config;
using Lunggo.Framework.Cors;
using Lunggo.WebAPI.ApiSrc.Newsletter.Model;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;

namespace Lunggo.WebAPI.ApiSrc.Newsletter
{
    public class NewsletterController : ApiController
    {
        [LunggoCorsPolicy]
        [Route("api/v1/newsletter/subscribe")]
        [HttpPost]
        public bool NewsletterSubscribe(HttpRequestMessage httpRequest, [FromBody] NewsletterSubscribeInput input)
        {
            if (!ModelState.IsValid)
            {
                return false;
            }
            var client = CreateApiClient();
            var request = CreateApiRequest();
            var jsonBody = CreateApiJsonBody(input);
            request.AddJsonBody(jsonBody);
            var response = client.Execute(request);
            return IsApiResponseValid(response);

        }

        private bool IsApiResponseValid(IRestResponse response)
        {
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                if (!String.IsNullOrEmpty(response.Content))
                {
                    var jObject = JObject.Parse(response.Content);
                    var errorReason = (String) jObject["title"];
                    return errorReason == "Member Exists";
                }
                else
                {
                    return false;
                }
            }
        }

        private RestClient CreateApiClient()
        {
            var path = ConfigManager.GetInstance().GetConfigValue("mailchimp", "addMemberApiRootUrl").Trim();
            var basicAuthUserName = ConfigManager.GetInstance().GetConfigValue("mailchimp", "basicAuthUserName").Trim();
            var basicAuthPassword = ConfigManager.GetInstance().GetConfigValue("mailchimp", "basicAuthPassword").Trim();
            
            var client = new RestClient(path)
            {
                Authenticator = new HttpBasicAuthenticator(basicAuthUserName, basicAuthPassword)
            };
            return client;
        }

        private RestRequest CreateApiRequest()
        {
            var path = ConfigManager.GetInstance().GetConfigValue("mailchimp", "addMemberApiPath").Trim();
            var request = new RestRequest(path, Method.POST);
            return request;
        }

        private MailchimpSubscriberJson CreateApiJsonBody(NewsletterSubscribeInput input)
        {
            //var nameSplitted = input.Name.Trim().Split((char[]) null);
            //var firstName = nameSplitted[0];
            //var lastName = nameSplitted.Length > 1 ? String.Join(" ", nameSplitted.Skip(1)) : nameSplitted[0];

            var jsonBody = new MailchimpSubscriberJson
            {
                status = "subscribed",
                email_address = input.Address.Trim().ToLowerInvariant(),
                merge_fields = new MergeFields
                {
                    FNAME = "",
                    LNAME = ""
                }
            };
            return jsonBody;
        }
        
    }
    
    public class MailchimpSubscriberJson
    {
        public String email_address { get; set; }
        public String status { get; set; }
        public MergeFields merge_fields { get; set; }
    }

    public class MergeFields
    {
        public String FNAME { get; set; }
        public String LNAME { get; set; }
    }

}
