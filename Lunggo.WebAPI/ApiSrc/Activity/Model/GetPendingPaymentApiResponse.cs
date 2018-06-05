using Lunggo.ApCommon.Activity.Model;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lunggo.WebAPI.ApiSrc.Activity.Model
{
    public class GetPendingPaymentApiResponse : ApiResponseBase
    {
        [JsonProperty("pendingPaymentList", NullValueHandling = NullValueHandling.Ignore)]
        public List<PendingPaymentForDisplay> PendingPaymentList { get; set; }
    }
}