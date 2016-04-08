using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.WebAPI.ApiSrc.v1.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.v1.Payment.Model
{
    public class PaymentApiResponse : ApiResponseBase
    {
        [JsonProperty("payst", NullValueHandling = NullValueHandling.Ignore)]
        public PaymentStatus? PaymentStatus { get; set; }
        [JsonProperty("met", NullValueHandling = NullValueHandling.Ignore)]
        public PaymentMethod? Method { get; set; }
        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public string RedirectionUrl { get; set; }
        [JsonProperty("acc", NullValueHandling = NullValueHandling.Ignore)]
        public string TransferAccount { get; set; }
    }
}