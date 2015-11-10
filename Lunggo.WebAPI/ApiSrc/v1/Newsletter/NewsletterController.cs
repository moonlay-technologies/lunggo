using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Lunggo.Framework.Cors;
using Lunggo.WebAPI.ApiSrc.v1.Newsletter.Model;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;

namespace Lunggo.WebAPI.ApiSrc.v1.Newsletter
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

        //TODO Bikin di Lunggo.Config
        private RestClient CreateApiClient()
        {
            var path = "http://us11.api.mailchimp.com";
            //
            var client = new RestClient(path)
            {
                Authenticator = new HttpBasicAuthenticator("travorama", "ad2872c0ab96857c93f3d69fdc88026f-us11")
            };
            return client;
        }

        //TODO bikin di Lunggo.CONFIG
        private RestRequest CreateApiRequest()
        {
            var path = "3.0/lists/4997f6c614/members";
            //
            var request = new RestRequest(path, Method.POST);
            return request;
        }

        private MailchimpSubscriberJson CreateApiJsonBody(NewsletterSubscribeInput input)
        {
            var nameSplitted = input.Name.Trim().Split((char[]) null);
            var firstName = nameSplitted[0];
            var lastName = nameSplitted.Length > 1 ? String.Join(" ", nameSplitted.Skip(1)) : nameSplitted[0];

            var jsonBody = new MailchimpSubscriberJson
            {
                status = "subscribed",
                email_address = input.Address.Trim().ToLowerInvariant(),
                merge_fields = new MergeFields
                {
                    FNAME = firstName,
                    LNAME = lastName
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
