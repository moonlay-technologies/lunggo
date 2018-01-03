using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net.Http;
using System.Net;

namespace Lunggo.ApCommon.Payment.Model
{
    public class DeleteRsvFromCartOutput
    {
        public HttpStatusCode StatusCode;
        public string ErrorCode;
    }
}
