using Lunggo.ApCommon.Activity.Model;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lunggo.WebAPI.ApiSrc.Activity.Model
{
    public class GetPendingRefundApiResponse : ApiResponseBase
    {
        [JsonProperty("refunds", NullValueHandling = NullValueHandling.Ignore)]
        public List<PendingRefund> PendingRefunds { get; set; }
    }
}