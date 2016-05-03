using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.v1.Payment.Model
{
    public class PayApiRequest
    {
        [JsonProperty("met")]
        public PaymentMethod Method { get; set; }
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public PaymentData Data { get; set; }
        [JsonProperty("tkn")]
        public string DiscountCode { get; set; }
        [JsonProperty("rsvno")]
        public string RsvNo { get; set; }
    }
}