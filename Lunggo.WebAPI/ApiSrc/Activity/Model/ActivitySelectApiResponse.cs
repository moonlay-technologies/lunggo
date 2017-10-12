using Lunggo.WebAPI.ApiSrc.Common.Model;
using System.Collections.Generic;
using Lunggo.ApCommon.Activity.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Activity.Model
{
    public class ActivitySelectApiResponse : ApiResponseBase
    {
        [JsonProperty("activityDetail", NullValueHandling = NullValueHandling.Ignore)]
        public ActivityDetailForDisplay ActivityDetail { get; set; }
    }
}