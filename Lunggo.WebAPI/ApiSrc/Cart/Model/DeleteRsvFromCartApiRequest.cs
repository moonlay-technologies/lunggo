using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Lunggo.ApCommon.Product.Model;
using Newtonsoft.Json;
using Lunggo.WebAPI.ApiSrc.Common.Model;

namespace Lunggo.WebAPI.ApiSrc.Cart.Model
{
    public class DeleteRsvFromCartApiRequest : ApiRequestBase
    {
        [JsonProperty("rsvNo")]
        public string RsvNo;
    }
}