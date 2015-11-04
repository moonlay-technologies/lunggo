using System;
using System.Collections.Generic;
using System.Text;

namespace Lunggo.Framework.Http.Rest
{
    public interface IRestRequest
    {
        HttpMethod Method { get; set; }
        Dictionary<String, String> Headers { get; }
        Dictionary<String, String> Parameters { get; }
        String RequestUri { get; set; }
        List<HttpFile> Files { get; }
        HttpRequestBody RequestBody { get; set; }
        bool AlwaysMultipartFormData { get; set; }
        void AddFile(String name,String fileName, byte[] fileInByte, String contentType);
        void AddParameter(String name, String value);
        void AddHeader(String name, String value);
        void AddBody(String value, Encoding encoding, String contentType);
        void AddBody(HttpRequestBody requestBody);
    }
}
