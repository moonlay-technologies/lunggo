using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;

namespace Lunggo.Framework.Http
{
    public class HttpServiceImpl : HttpService
    {
        private HttpClient httpClient;

        public HttpServiceImpl(String uri,HttpServiceOptions serviceOptions)
        {
            Uri = uri;
            Initialize(serviceOptions);
        }

        public T GetAPICall<T>()
        {
            throw new NotImplementedException();
        }

        public T PostAPICall<T>()
        {
            throw new NotImplementedException();
        }

        public T PutAPICall<T>()
        {
            throw new NotImplementedException();
        }

        public T DeleteAPICall<T>()
        {
            throw new NotImplementedException();
        }

        public Task<T> GetAPICallAsync<T>()
        {
            throw new NotImplementedException();
        }

        public Task<T> PostAPICallAsync<T>()
        {
            throw new NotImplementedException();
        }

        public Task<T> PutAPICallAsync<T>()
        {
            throw new NotImplementedException();
        }

        public Task<T> DeleteAPICallAsync<T>()
        {
            throw new NotImplementedException();
        }

        private void Initialize(HttpServiceOptions serviceOptions)
        {
            this.httpClient = CreateHttpClient(serviceOptions);
        }

        private HttpClient CreateHttpClient(HttpServiceOptions serviceOptions)
        {
            HttpClientHandler handler = CreateHttpClientHandler(serviceOptions);
            HttpClient client = CreateHttpClient(handler);
            return client;
        }

        private HttpClient CreateHttpClient(HttpClientHandler handler)
        {
            return new HttpClient(handler);
        }

        private HttpClientHandler CreateHttpClientHandler(HttpServiceOptions serviceOptions)
        {
            var handler = new HttpClientHandler();

            if ((serviceOptions & HttpServiceOptions.CookieEnabled) != 0)
            {
                EnableCookieOptionsInHttpHandler(handler);
            }

            if ((serviceOptions & HttpServiceOptions.ProxyEnabled) != 0)
            {
                EnableProxyOptionsInHttpHandler(handler);
            }

            if ((serviceOptions & HttpServiceOptions.RedirectEnabled) != 0)
            {
                EnableRedirectOptionsInHttpHandler(handler);
            }

            return handler;
        }

        private void EnableCookieOptionsInHttpHandler(HttpClientHandler handler)
        {
            var cookiesContainer = new CookieContainer();
            handler.CookieContainer = cookiesContainer;
            handler.UseCookies = true;
        }

        private void EnableProxyOptionsInHttpHandler(HttpClientHandler handler)
        {
            throw new NotImplementedException();
        }

        private void EnableRedirectOptionsInHttpHandler(HttpClientHandler handler)
        {
            handler.AllowAutoRedirect = true;
            handler.MaxAutomaticRedirections = 2;
        }
    }
}
