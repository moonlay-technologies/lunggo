using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lunggo.WebAPI.ApiSrc.Activity.Model
{
    public class MarkPaymentAsFailedApiRequest
    {
        [JsonProperty("rsvNo", NullValueHandling = NullValueHandling.Ignore)]
        public string RsvNo { get; set; }
        [JsonProperty("pendingPaymentStatus", NullValueHandling = NullValueHandling.Ignore)]
        public string PendingPaymentStatus { get; set; }
    }
}