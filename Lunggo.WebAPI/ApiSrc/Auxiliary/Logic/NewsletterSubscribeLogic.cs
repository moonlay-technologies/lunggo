using System;
using System.Net;
using Lunggo.Framework.Environment;
using Lunggo.WebAPI.ApiSrc.Auxiliary.Model;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using RestSharp;
using RestSharp.Authenticators;

namespace Lunggo.WebAPI.ApiSrc.Auxiliary.Logic
{
    public static partial class AuxiliaryLogic
    {
        public static ApiResponseBase NewsletterSubscribe(NewsletterSubscribeApiRequest request)
        {
            if (!IsValid(request))
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorCode = "ERXSUB01"
                };

            var client = CreateApiClient();
            var httpRequest = CreateApiRequest();
            var jsonBody = CreateApiJsonBody(request);
            httpRequest.AddJsonBody(jsonBody);
            var httpResponse = client.Execute(httpRequest);
            return AssembleApiResponse(httpResponse);

        }

        private static ApiResponseBase AssembleApiResponse(IRestResponse httpResponse)
        {
            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                return new ApiResponseBase
                {
                    StatusCode = HttpStatusCode.OK
                };
            }
            else
            {
                if (!String.IsNullOrEmpty(httpResponse.Content))
                {
                    return new ApiResponseBase
                    {
                        StatusCode = HttpStatusCode.Accepted,
                        ErrorCode = "ERXSUB02"
                    };
                }
                else
                {
                    return ApiResponseBase.Error500();
                }
            }
        }

        private static bool IsValid(NewsletterSubscribeApiRequest request)
        {
            return
                request != null &&
                request.Email != null &&
                IsEmailFormatValid(request.Email);
        }

        private static bool IsEmailFormatValid(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private static RestClient CreateApiClient()
        {
            var path = EnvVariables.Get("mailchimp", "addMemberApiRootUrl").Trim();
            var basicAuthUserName = EnvVariables.Get("mailchimp", "basicAuthUserName").Trim();
            var basicAuthPassword = EnvVariables.Get("mailchimp", "basicAuthPassword").Trim();

            var client = new RestClient(path)
            {
                Authenticator = new HttpBasicAuthenticator(basicAuthUserName, basicAuthPassword)
            };
            return client;
        }

        private static RestRequest CreateApiRequest()
        {
            var path = EnvVariables.Get("mailchimp", "addMemberApiPath").Trim();
            var request = new RestRequest(path, Method.POST);
            return request;
        }

        private static MailchimpSubscriberJson CreateApiJsonBody(NewsletterSubscribeApiRequest apiRequest)
        {
            string firstName, lastName;
            if (apiRequest.Name == null)
            {
                firstName = lastName = "";
            }
            else
            {
                var splittedName = apiRequest.Name.Trim().Split(' ');

                if (splittedName.Length <= 1)
                {
                    firstName = lastName = splittedName[0];
                }
                else
                {
                    lastName = splittedName[splittedName.Length - 1];
                    firstName = apiRequest.Name.Substring(0, apiRequest.Name.LastIndexOf(' '));
                }
            }

            var jsonBody = new MailchimpSubscriberJson
            {
                status = "subscribed",
                email_address = apiRequest.Email.Trim().ToLowerInvariant(),
                merge_fields = new MergeFields
                {
                    FNAME = firstName,
                    LNAME = lastName
                }
            };
            return jsonBody;
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
}