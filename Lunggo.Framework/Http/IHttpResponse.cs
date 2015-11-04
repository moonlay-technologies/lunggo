using System;

namespace Lunggo.Framework.Http
{
    public interface IHttpResponse
    {
        String RawString { get; set; }
    }

    interface IHttpResponse<T> : IHttpResponse
    {
        T Data { get; set; }
    }
}
