using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lunggo.Framework.Http.Rest
{
    public class RestRequest : IRestRequest
    {
        public HttpMethod Method { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public Dictionary<string, string> Parameters { get; set; }
        public String RequestUri { get; set; }
        public List<HttpFile> Files { get; set; }
        public bool AlwaysMultipartFormData { get; set; }
        public HttpRequestBody RequestBody { get; set; }
        
        public RestRequest()
        {
            Headers = new Dictionary<string, string>();
            Parameters = new Dictionary<string, string>();
            Files = new List<HttpFile>();
        }
        
        public void AddFile(String name,String fileName, byte[] fileInByte, String contentType)
        {
            HttpFile newFile = new HttpFile 
            {
                Name = name,
                FileName = fileName,
                ContentLength = fileInByte.Length,
                ContentType = contentType,
                Data = fileInByte
            };
            Files.Add(newFile);    
        }
        public void AddParameter(String name, String value)
        {
            Parameters.Add(name, value);
        }
        public void AddHeader(String name, String value)
        {
            Headers.Add(name, value);
        }
        public void AddBody(String value, Encoding encoding, String contentType)
        {
            HttpRequestBody requestBody = new HttpRequestBody(encoding);
            requestBody.SetBody(value, contentType);
            RequestBody = requestBody;
        }
        public void AddBody(HttpRequestBody requestBody)
        {
            RequestBody = requestBody;
        }


    }
}
