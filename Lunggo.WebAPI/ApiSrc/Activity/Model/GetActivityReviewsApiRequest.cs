using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lunggo.WebAPI.ApiSrc.Activity.Model
{
    public class GetActivityReviewsApiRequest
    {
        [JsonProperty("activityId", NullValueHandling = NullValueHandling.Ignore)]
        public long? ActivityId { get; set; }
    }
}