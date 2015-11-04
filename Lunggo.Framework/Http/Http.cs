using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Lunggo.Framework.Http
{
    public class Http : IHttp
    {
        private static readonly int _defaultTimeout = 5000;
        public int Timeout { get; set; }
        public String BaseAddress { get; set; }
        public CookieContainer CookieContainer { get; set; }
        public IWebProxy Proxy { get; set; }

        public Http()
        {
            Timeout = _defaultTimeout;
        }

        public IHttpResponse SendRequest(IHttpRequest httpRequest)
        {
            HttpResponseMessage response = GetHttpClient().SendAsync(GetHttpRequestMessage(httpRequest)).Result;
            return CreateHttpResponse(response);
        }

        private IHttpResponse CreateHttpResponse(HttpResponseMessage httpResponseMessage)
        {
            IHttpResponse response = new HttpResponse();
            response.RawString = httpResponseMessage.Content.ReadAsStringAsync().Result;
            return response;
        }

        private HttpRequestMessage GetHttpRequestMessage(IHttpRequest httpRequest)
        {
            HttpRequestMessage message = new HttpRequestMessage();
            ConfigureHttpRequestMessage(message, httpRequest);
            return message;
        }

        private void ConfigureHttpRequestMessage(HttpRequestMessage message, IHttpRequest httpRequest)
        {
            SetHttpRequestMessageUri(message, httpRequest);
            SetHttpRequestMessageMethod(message,httpRequest);
            SetHttpRequestMessageHeaders(message.Headers,httpRequest.Headers);
            AddHttpContent(message, httpRequest);
        }

        private void SetHttpRequestMessageUri(HttpRequestMessage message, IHttpRequest httpRequest)
        { 
            if(httpRequest.Method == HttpMethod.GET)
            {
               message.RequestUri = CreateGETUri(BaseAddress, httpRequest.RequestUri, httpRequest.Parameters);
            }
            else
            {
                message.RequestUri = CreateUri(BaseAddress, httpRequest.RequestUri);
            }
        }

        private Uri CreateGETUri(String baseAddress,String requestUri, List<HttpParameter> parameterList)
        {
            return new Uri(HttpUtil.CreateUrlWithParameter(baseAddress + requestUri,parameterList));
        }

        private Uri CreateUri(String baseAddress, String requestUri)
        {
            return new Uri(baseAddress + requestUri);
        }

        private void SetHttpRequestMessageMethod(HttpRequestMessage message, IHttpRequest httpRequest)
        {
            HttpMethod method = httpRequest.Method;
            System.Net.Http.HttpMethod httpRequestMessageMethod = null;
            
            if(method == HttpMethod.POST)
            {
                httpRequestMessageMethod = System.Net.Http.HttpMethod.Post;
            }
            else if(method == HttpMethod.GET)
            {
                httpRequestMessageMethod = System.Net.Http.HttpMethod.Get;
            }

            message.Method = httpRequestMessageMethod;
        }

        private void AddHttpContent(HttpRequestMessage message,IHttpRequest httpRequest)
        {
            message.Content = CreateHttpContent(httpRequest);
        }

        private HttpContent CreateHttpContent(IHttpRequest httpRequest)
        {
            HttpContent content = null;
            if(IsEligibleForMultipartFormDataContent(httpRequest))
            {
                content = CreateMultipartFormDataContent(httpRequest);
            }
            else if(IsEligibleForFormUrlEncodedContent(httpRequest))
            {
                content = CreateFormUrlEncodedContent(httpRequest.Parameters);
            }
            else if(IsEligibleForRequestBody(httpRequest))
            {
                content = CreateHttpContentFromRequestBody(httpRequest);
            }
            return content;
        }

        private bool IsEligibleForMultipartFormDataContent(IHttpRequest httpRequest)
        {
            if(httpRequest.Method == HttpMethod.POST)
            {
                return (httpRequest.HasFiles || httpRequest.AlwaysMultipartFormData) ? true : false;
            }
            else
            {
                return false;
            }
        }

        private bool IsEligibleForFormUrlEncodedContent(IHttpRequest httpRequest)
        {
            if (httpRequest.Method == HttpMethod.POST)
            {
                return httpRequest.HasParameters;
            }
            else
            {
                return false;
            }
        }

        private bool IsEligibleForRequestBody(IHttpRequest httpRequest)
        {
            if(httpRequest.Method == HttpMethod.POST)
            {
                return httpRequest.HasBody;
            }
            else
            {
                return false;
            }
        }

        private HttpContent CreateHttpContentFromRequestBody(IHttpRequest httpRequest)
        {
            return CreateByteArrayContent(httpRequest.RequestBody);
        }

        private HttpContent CreateMultipartFormDataContent(IHttpRequest httpRequest)
        {
            var multipartContent = new MultipartFormDataContent();

            if(httpRequest.HasParameters)
            {
                AddFormParametertoMultipartContent(multipartContent,httpRequest.Parameters);
            }

            if(httpRequest.HasFiles)
            {
                AddFiletoMultipartContent(multipartContent, httpRequest.Files);
            }
            return multipartContent;
        }

        private void AddFiletoMultipartContent(MultipartFormDataContent content, List<HttpFile> fileList)
        {
            foreach (var file in fileList)
            {
                content.Add(CreateByteArrayContentForMultipartContent(file));
            }
        }

        private HttpContent CreateByteArrayContentForMultipartContent(HttpFile file)
        {
            var fileContent = new ByteArrayContent(file.Data);
            fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
            {
                FileName = file.FileName,
                Name = file.Name
            };
            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
            return fileContent;
        }

        private HttpContent CreateByteArrayContent(byte[] contentInBytes, String contentType)
        {
            var byteArrayContent = new ByteArrayContent(contentInBytes);
            byteArrayContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);
            byteArrayContent.Headers.ContentLength = contentInBytes.Length;
            return byteArrayContent;
        }

        private HttpContent CreateByteArrayContent(HttpRequestBody body)
        {
            return CreateByteArrayContent(body.GetBody(), body.ContentType);
        }

        private void AddFormParametertoMultipartContent(MultipartFormDataContent content, List<HttpParameter> paramList)
        {
            foreach (var parameter in paramList)
            {
                content.Add(new StringContent(parameter.Value), parameter.Name);
            }
        }

        private HttpContent CreateFormUrlEncodedContent(List<HttpParameter> paramList)
        {
            IList<KeyValuePair<String, String>> keyValueList = new List<KeyValuePair<String,String>>();
            foreach(var parameter in paramList)
            {
                keyValueList.Add(new KeyValuePair<String,String>(parameter.Name,parameter.Value));
            }
            return new FormUrlEncodedContent(keyValueList);
        }


        private void SetHttpRequestMessageHeaders(HttpRequestHeaders httpRequestMessageHeaders, List<HttpHeader> requestHeaders)
        {
            ClearHttpRequestMessageHeaders(httpRequestMessageHeaders);
            InsertDefaultHttpRequestMessageheaders(httpRequestMessageHeaders);
            InsertRequestHeaders(httpRequestMessageHeaders, requestHeaders);
        }
        private void InsertDefaultHttpRequestMessageheaders(HttpRequestHeaders httpRequestMessageHessageHeaders)
        {
            //No default headers at the moment
        }   
        private void InsertRequestHeaders(HttpRequestHeaders httpRequestMessageHeaders, List<HttpHeader> requestHeaders)
        {
            if(requestHeaders != null)
            {
                foreach (var requestHeader in requestHeaders)
                {
                    if (!httpRequestMessageHeaders.Contains(requestHeader.Name))
                    {
                        httpRequestMessageHeaders.Add(requestHeader.Name, requestHeader.Value);
                    }
                }
            }
        }

        private void ClearHttpRequestMessageHeaders(HttpRequestHeaders headers)
        {
            headers.Clear();
        }

        private HttpClient GetHttpClient()
        {
            var client = CreateHttpClient();
            ConfigureHttpClient(client);
            return client;
        }

        private HttpClient CreateHttpClient()
        {
            var handler = CreateHttpClientHandler();
            var client = new HttpClient(handler);
            return client;
        }

        private void ConfigureHttpClient(HttpClient client)
        {
            client.Timeout = TimeSpan.FromMilliseconds((double)Timeout);
            client.DefaultRequestHeaders.Clear();
        }

        private HttpClientHandler CreateHttpClientHandler()
        {
           var handler = new HttpClientHandler(); 
           SetHandlerDefaultCredentials(handler);
           SetHandlerCookieContainer(handler);
           SetHandlerProxy(handler);
           return handler;
        }

        private void SetHandlerDefaultCredentials(HttpClientHandler handler)
        {
            handler.UseDefaultCredentials = false;
        }

        private void SetHandlerCookieContainer(HttpClientHandler handler)
        {
            if (CookieContainer != null)
            {
                handler.UseCookies = true;
                handler.CookieContainer = CookieContainer;
            }
            else
            {
                handler.UseCookies = false;
            }
        }

        private void SetHandlerProxy(HttpClientHandler handler)
        {
            if(Proxy != null)
            {
                handler.UseProxy = true;
                handler.Proxy = Proxy;
            }
            else
            {
                handler.UseProxy = false;
                handler.Proxy = null;
            }
        }
    }
}
