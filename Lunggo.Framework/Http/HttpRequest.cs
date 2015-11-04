using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.Framework.Http.Rest;

namespace Lunggo.Framework.Http
{
    public class HttpRequest : IHttpRequest
    {
        public List<HttpHeader> Headers { get; set; }
        public List<HttpParameter> Parameters { get; set; }
        public List<HttpFile> Files { get; set; }
        public bool AlwaysMultipartFormData {get; set;}
        public String RequestUri { get; set; }
        public HttpMethod Method { get; set; }
        public HttpRequestBody RequestBody { get; set; }
        public bool HasFiles 
        { 
            get
            {
                return Files == null ? false : Files.Any();
            }
        }
        public bool HasParameters 
        { 
            get
            {
                return Parameters == null ? false : Parameters.Any();
            }
        }

        public bool HasBody
        { 
            get
            {
                return RequestBody != null;
            }
        }

        public HttpRequest()
        {
            Headers = new List<HttpHeader>();
            Parameters = new List<HttpParameter>();
            Files = new List<HttpFile>();
        }

        public static  HttpRequest CreateHttpRequest(IRestRequest restRequest)
        {
            HttpRequest httpRequest = new HttpRequest();
            CopyRestRequestToHttpRequest(httpRequest, restRequest);
            return httpRequest;
        }

        private static void CopyRestRequestToHttpRequest(HttpRequest httpRequest, IRestRequest restRequest)
        {
            CopyHeaders(httpRequest,restRequest);
            CopyParameters(httpRequest, restRequest);
            CopyFiles(httpRequest, restRequest);
            CopyOtherProperties(httpRequest, restRequest);
        }

        private static void CopyOtherProperties(HttpRequest httpRequest, IRestRequest restRequest)
        {
            httpRequest.AlwaysMultipartFormData = restRequest.AlwaysMultipartFormData;
            httpRequest.RequestUri = restRequest.RequestUri;
            httpRequest.Method = restRequest.Method;
            httpRequest.RequestBody = restRequest.RequestBody;
        }

        private static void CopyHeaders(HttpRequest httpRequest, IRestRequest restRequest)
        {
            foreach (KeyValuePair<String, String> keyValuePair in restRequest.Headers)
            {
                httpRequest.Headers.Add(new HttpHeader
                    {
                        Name = keyValuePair.Key,
                        Value = keyValuePair.Value
                    }
                );
            }
        }

        private static void CopyParameters(HttpRequest httpRequest, IRestRequest restRequest)
        {
            foreach (KeyValuePair<String, String> keyValuePair in restRequest.Parameters)
            {
                httpRequest.Parameters.Add(new HttpParameter
                {
                    Name = keyValuePair.Key,
                    Value = keyValuePair.Value
                }
                );
            }
        }

        private static void CopyFiles(HttpRequest httpRequest, IRestRequest restRequest)
        {
            foreach (HttpFile file in restRequest.Files)
            {
                httpRequest.Files.Add(file);
            }
        }
    }
}
