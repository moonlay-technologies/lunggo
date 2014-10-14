using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Lunggo.Framework.SharedModel;
using Newtonsoft.Json;
using RestSharp;

namespace Lunggo.Framework.TicketSupport.ZendeskClass
{

    public interface ICore
    {
		T GetByPageUrl<T>(string pageUrl, int perPage=100);
		T RunRequest<T>(string resource, string requestMethod, object body = null);
        ZendeskRequestResult RunRequest(string resource, string requestMethod, object body = null);

		Task<T> GetByPageUrlAsync<T>(string pageUrl, int perPage = 100);
		Task<T> RunRequestAsync<T>(string resource, string requestMethod, object body = null);
        Task<ZendeskRequestResult> RunRequestAsync(string resource, string requestMethod, object body = null);
    }

    public class ZendeskCore : ICore
    {
        private const string XOnBehalfOfEmail = "X-On-Behalf-Of";
        protected string User;
        protected string Password;
        protected string ZendeskUrl;
        protected string ApiToken;


        /// <summary>
        /// Constructor that uses BasicHttpAuthentication.
        /// </summary>
        /// <param name="zendeskApiUrl"></param>
        /// <param name="user"></param>
        /// <param name="password">LEAVE BLANK IF USING TOKEN</param>
        /// <param name="apiToken">Optional Param which is used if specified instead of the password</param>
        public ZendeskCore(string zendeskApiUrl, string user, string password, string apiToken)
        {
            User = user;
            Password = password;
            ZendeskUrl = zendeskApiUrl;
            ApiToken = apiToken;
        }

        internal IWebProxy Proxy;

        public T GetByPageUrl<T>(string pageUrl, int perPage=100)
        {            
            if(string.IsNullOrEmpty(pageUrl))
                return JsonConvert.DeserializeObject<T>("");

            var resource = Regex.Split(pageUrl, "api/v2/").Last() + "&per_page=" + perPage;
            return RunRequest<T>(resource, "GET");
        }

        public T RunRequest<T>(string resource, string requestMethod, object body = null)
        {
            var response = RunRequest(resource, requestMethod, body);
            var obj = JsonConvert.DeserializeObject<T>(response.Content, new JsonSerializerSettings()
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
            return obj;
        }

        //public ZendeskRequestResult RunRequest(string resource, string requestMethod, object body = null)
        //{
        //    try
        //    {
        //        var requestUrl = ZendeskUrl;
        //        if (!requestUrl.EndsWith("/"))
        //            requestUrl += "/";

        //        requestUrl += resource;

        //        HttpWebRequest req = WebRequest.Create(requestUrl) as HttpWebRequest;
        //        req.ContentType = "application/json";

        //    if (this.Proxy != null)
        //        req.Proxy = this.Proxy;
                
        //        //req.Credentials = new NetworkCredential(User, Password);
        //        //req.Credentials = new System.Net.CredentialCache
        //        //                      {
        //        //                          {
        //        //                              new System.Uri(ZendeskUrl), "Basic",
        //        //                              new System.Net.NetworkCredential(User, Password)
        //        //                              }
        //        //                      };

        //        req.Headers["Authorization"] = GetPasswordOrTokenAuthHeader();
        //        req.PreAuthenticate = true;

        //        req.Method = requestMethod; //GET POST PUT DELETE
        //        req.Accept = "application/json, application/xml, text/json, text/x-json, text/javascript, text/xml";
        //        req.ContentLength = 0;

        //        if (body != null)
        //        {
        //            var json = JsonConvert.SerializeObject(body, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
        //            byte[] formData = UTF8Encoding.UTF8.GetBytes(json);
        //            req.ContentLength = formData.Length;

        //            var dataStream = req.GetRequestStream();
        //            dataStream.Write(formData, 0, formData.Length);
        //            dataStream.Close();
        //        }
        //        var res = req.GetResponse();
        //        HttpWebResponse response = res as HttpWebResponse;
        //        var responseStream = response.GetResponseStream();
        //        var reader = new StreamReader(responseStream);
        //        string responseFromServer = reader.ReadToEnd();

        //        return new ZendeskRequestResult()
        //        {
        //            Content = responseFromServer,
        //            HttpStatusCode = response.StatusCode
        //        };
        //    }
        //    catch (WebException ex)
        //    {
        //        throw new WebException(ex.Message + " " + ex.Response.Headers.ToString(), ex);
        //    }            
        //}       
        public ZendeskRequestResult RunRequest(string resource, string requestMethod, object body = null)
        {
            try
            {


                var requestUrl = ZendeskUrl;
                if (!requestUrl.EndsWith("/"))
                    requestUrl += "/";

                RestClient restClientRestSharp = new RestClient(requestUrl);
                var requestRestSharp = new RestRequest(resource, Method.POST);
                requestRestSharp.AddHeader("Authorization", GetPasswordOrTokenAuthHeader());
                requestRestSharp.AddHeader("Accept", "application/json");
                requestRestSharp.AddHeader("Content-Type", "application/json");
                requestRestSharp.XmlSerializer.ContentType = "application/json";
                requestRestSharp.RequestFormat = DataFormat.Json;


                if (body != null)
                {
                    var json = JsonConvert.SerializeObject(body, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
                    requestRestSharp.AddParameter("application/json", json, ParameterType.RequestBody);
                }
                var responses = restClientRestSharp.Execute(requestRestSharp);
                return new ZendeskRequestResult()
                {
                    Content = responses.Content,
                    HttpStatusCode = responses.StatusCode
                };
            }
            catch (WebException ex)
            {
                throw new WebException(ex.Message + " " + ex.Response.Headers.ToString(), ex);
            }
        }

        protected T GenericGet<T>(string resource)
        {
            return RunRequest<T>(resource, "GET");            
        }
        
        protected bool GenericDelete(string resource)
        {
            var res = RunRequest(resource, "DELETE");
            return res.HttpStatusCode == HttpStatusCode.OK;            
        }
        
        protected T GenericPost<T>(string resource, object body=null)
        {
            var res = RunRequest<T>(resource, "POST", body);            
            return res;
        }

        protected bool GenericBoolPost(string resource, object body = null)
        {
            var res = RunRequest(resource, "POST", body);
            return res.HttpStatusCode == HttpStatusCode.OK;
        }

        protected T GenericPut<T>(string resource, object body=null)
        {
            var res = RunRequest<T>(resource, "PUT", body);
            return res;
        }

        protected bool GenericBoolPut(string resource, object body = null)
        {
            var res = RunRequest(resource, "PUT", body);
            return res.HttpStatusCode == HttpStatusCode.OK;
        }
        protected string GetPasswordOrTokenAuthHeader()
        {
            if (!String.IsNullOrEmpty(ApiToken) && ApiToken.Trim().Length >= 0)
                return GetAuthHeader(User + "/token", ApiToken);

            return GetAuthHeader(User, Password);
        }

        protected string GetAuthHeader(string userName, string password)
        {
            string auth = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", userName, password)));
            return string.Format("Basic {0}", auth);
        }
        public async Task<T> GetByPageUrlAsync<T>(string pageUrl, int perPage = 100)
        {
            if (string.IsNullOrEmpty(pageUrl))
                return JsonConvert.DeserializeObject<T>("");

            var resource = Regex.Split(pageUrl, "api/v2/").Last() + "&per_page=" + perPage;
            return await RunRequestAsync<T>(resource, "GET");
        }

        public async Task<T> RunRequestAsync<T>(string resource, string requestMethod, object body = null)
        {
            var response = await RunRequestAsync(resource, requestMethod, body);
            var obj = Task.Factory.StartNew(() => JsonConvert.DeserializeObject<T>(response.Content));
            return await obj;
        }

        public async Task<ZendeskRequestResult> RunRequestAsync(string resource, string requestMethod, object body = null)
        {
            var requestUrl = ZendeskUrl;
            if (!requestUrl.EndsWith("/"))
                requestUrl += "/";

            requestUrl += resource;

            HttpWebRequest req = WebRequest.Create(requestUrl) as HttpWebRequest;
            req.ContentType = "application/json";

            //req.Credentials = new System.Net.NetworkCredential(User, Password);
            req.Headers["Authorization"] = GetPasswordOrTokenAuthHeader();


            req.Method = requestMethod; //GET POST PUT DELETE
            req.Accept = "application/json, application/xml, text/json, text/x-json, text/javascript, text/xml";

            if (body != null)
            {
                var json = JsonConvert.SerializeObject(body, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
                byte[] formData = UTF8Encoding.UTF8.GetBytes(json);

                var requestStream = Task.Factory.FromAsync(
                    req.BeginGetRequestStream,
                    asyncResult => req.EndGetRequestStream(asyncResult),
                    (object)null);

                var dataStream = await requestStream.ContinueWith(t => t.Result.WriteAsync(formData, 0, formData.Length));
                Task.WaitAll(dataStream);
            }
            
            Task<WebResponse> task = Task.Factory.FromAsync(
            req.BeginGetResponse,
            asyncResult => req.EndGetResponse(asyncResult),
            (object)null);

            return await task.ContinueWith(t =>
            {
                var httpWebResponse = t.Result as HttpWebResponse;

                return new ZendeskRequestResult
                {
                    Content = ReadStreamFromResponse(httpWebResponse),
                    HttpStatusCode = httpWebResponse.StatusCode
                };

            });
        }

        private static string ReadStreamFromResponse(WebResponse response)
        {
            using (Stream responseStream = response.GetResponseStream())
            using (StreamReader sr = new StreamReader(responseStream))
            {
                //Need to return this response 
                string strContent = sr.ReadToEnd();
                return strContent;
            }
        }

        protected async Task<T> GenericGetAsync<T>(string resource)
        {
            return await RunRequestAsync<T>(resource, "GET");
        }

        protected async Task<bool> GenericDeleteAsync(string resource)
        {
            var res = RunRequestAsync(resource, "DELETE");
            return await res.ContinueWith(x => x.Result.HttpStatusCode == HttpStatusCode.OK);
        }

        protected async Task<T> GenericPostAsync<T>(string resource, object body = null)
        {
            var res = RunRequestAsync<T>(resource, "POST", body);
            return await res;
        }

        protected async Task<bool> GenericBoolPostAsync(string resource, object body = null)
        {
            var res = RunRequestAsync(resource, "POST", body);
            return await res.ContinueWith(x => x.Result.HttpStatusCode == HttpStatusCode.OK);
        }

        protected async Task<T> GenericPutAsync<T>(string resource, object body = null)
        {
            var res = RunRequestAsync<T>(resource, "PUT", body);
            return await res;
        }

        protected async Task<bool> GenericBoolPutAsync(string resource, object body = null)
        {
            var res = RunRequestAsync(resource, "PUT", body);
            return await res.ContinueWith(x => x.Result.HttpStatusCode == HttpStatusCode.OK);
        }
    }
}
