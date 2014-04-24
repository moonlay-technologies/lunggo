using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.Framework.Http
{
    public interface IHttpRequest
    {
        HttpMethod Method { get; set; }
        List<HttpHeader> Headers { get; }
        List<HttpParameter> Parameters { get; }
        String RequestUri { get; set; }
        List<HttpFile> Files { get; set; }
        bool AlwaysMultipartFormData { get; set; }
        HttpRequestBody RequestBody { get; set; }
        bool HasFiles { get; }
        bool HasParameters { get; }
        bool HasBody { get; }
    }
}
