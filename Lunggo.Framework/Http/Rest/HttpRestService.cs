using System;
using System.Net;

namespace Lunggo.Framework.Http.Rest
{
    class HttpRestService
    {

        public IWebProxy HttpProxy { get; set; }
        public int Timeout { get; set; }
        public bool FollowRedirects { get; set; }
        public int MaxRedirect { get; set; }
        public CookieContainer CookieContainer { get; set; }
        public String BaseAddress { get; set; }

        private IHttp _httpClient = new Http();

        public IRestResponse Execute(IRestRequest restRequest)
        {
            ConfigureHttpClient();

            IRestResponse restResponse = ExecuteRestRequest(restRequest);

            return restResponse;
        }

        private IRestResponse ExecuteRestRequest(IRestRequest restRequest)
        {
           return null;
        }

        private IRestResponse ExecuteA(IRestRequest restRequest)
        {
            var httpMethod = restRequest.Method;
            return null;
            /*

            if(httpMethod == HttpMethod.GET)
            {
                return _httpClient.ExecuteGet(restRequest);
            }
            else if(httpMethod == HttpMethod.POST)
            {
                return _httpClien
            }
            else
            {
                Exception unsupportedHttpMethodException = new Exception("Unsupported Http Method");
                throw unsupportedHttpMethodException;
            }
            */
        }

        private void ConfigureHttpClient()
        {
            _httpClient.Timeout = this.Timeout;
            _httpClient.CookieContainer = this.CookieContainer;
            _httpClient.Proxy = this.HttpProxy;
            _httpClient.BaseAddress = this.BaseAddress;
        }   
    }
}
