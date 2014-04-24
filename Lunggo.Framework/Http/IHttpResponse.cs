using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
