using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.v1.Promo.Imlek.Model
{
    public class ImlekApiResponse
    {
        [JsonProperty("c")]
        public int ReturnCode { get; set; }
        [JsonProperty("v")]
        public string VoucherCode { get; set; }
    }
}