using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Lunggo.Framework.Cors;
using Lunggo.WebAPI.ApiSrc.v1.Promo.Imlek.Model;

namespace Lunggo.WebAPI.ApiSrc.v1.Promo
{
    public class PromoController : ApiController
    {
        [HttpPost]
        [LunggoCorsPolicy]
        [Route("v1/promo/imlek")]
        public ImlekApiResponse Imlek(HttpRequestMessage httpRequest, [FromBody] ImlekApiRequest request)
        {
            return new ImlekApiResponse
            {
                ReturnCode = new Random().Next(-1, 5)
            };
        }

    }
}
