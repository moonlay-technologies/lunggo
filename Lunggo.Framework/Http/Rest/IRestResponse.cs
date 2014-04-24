using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.Framework.Http.Rest
{
    interface IRestResponse
    {

    }

    interface IRestResponse<T> : IRestResponse
    {
        T Data { get; set; }
    }
}
