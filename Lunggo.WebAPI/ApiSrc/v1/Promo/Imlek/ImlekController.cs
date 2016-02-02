using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Lunggo.Framework.Cors;

namespace Lunggo.WebAPI.ApiSrc.v1.Promo.Imlek
{
    public class ImlekController : ApiController
    {
        [HttpPost]
        [LunggoCorsPolicy]
        [Route("v1/promo/imlek")]
        public int PromoImlek(HttpRequestMessage httpRequest, [FromUri] string email)
        {
            return new Random().Next(-1,5);
        }

    }
}
