using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Lunggo.Framework.Http
{
    public abstract class HttpService
    {
        public Dictionary<String, String> RequestParameters { get; set; }
        public Dictionary<String, String> RequestHeaders { get; set; }
        public String Uri { get; set; }
        
        
        public abstract T GetAPICall<T>();
        public abstract T PostAPICall<T>();
        public abstract T PutAPICall<T>();
        public abstract T DeleteAPICall<T>();
        public abstract Task<T> GetAPICallAsync<T>();
        public abstract Task<T> PostAPICallAsync<T>();
        public abstract Task<T> PutAPICallAsync<T>();
        public abstract Task<T> DeleteAPICallAsync<T>();
        public abstract void AddHeader(String headerKey, String headerValue);
        public abstract void AddParameter(String paramKey, String paramValue);

        
        public static HttpService Create(String uri)
        {
            return CreateService(uri, HttpServiceOptions.Default);
        }

        public static HttpService Create(String uri,HttpServiceOptions serviceOptions)
        {
            return CreateService(uri,serviceOptions);
        }

        private static HttpService CreateService(String uri, HttpServiceOptions serviceOptions)
        {
            return new HttpServiceImpl(uri, serviceOptions);
        }
    }

    [Flags]
    public enum HttpServiceOptions
    {
        Default = 0,
        CookieEnabled = 1,
        ProxyEnabled = 2,
        RedirectEnabled = 4
    }

}
