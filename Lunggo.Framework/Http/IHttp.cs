using System;
using System.Net;

namespace Lunggo.Framework.Http
{
    public interface IHttp
    {
        int Timeout { get; set; }
        String BaseAddress { get; set; }
        CookieContainer CookieContainer { get; set; }
        IWebProxy Proxy { get; set; }
        IHttpResponse SendRequest(IHttpRequest httpRequest);
    }

}
