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
        [JsonProperty("paymentStatus", NullValueHandling = NullValueHandling.Ignore)]
        public PaymentStatus? PaymentStatus { get; set; }
        [JsonProperty("method", NullValueHandling = NullValueHandling.Ignore)]
        public PaymentMethod? Method { get; set; }
        [JsonProperty("redirectionUrl", NullValueHandling = NullValueHandling.Ignore)]
        public string RedirectionUrl { get; set; }
        [JsonProperty("transferAccount", NullValueHandling = NullValueHandling.Ignore)]
        public string TransferAccount { get; set; }
    }
}