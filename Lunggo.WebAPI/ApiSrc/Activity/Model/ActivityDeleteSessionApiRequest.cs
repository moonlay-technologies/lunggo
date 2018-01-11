using Lunggo.ApCommon.Activity.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lunggo.WebAPI.ApiSrc.Activity.Model
{
    public class ActivityDeleteSessionApiRequest
    {
        [JsonProperty("activityId", NullValueHandling = NullValueHandling.Ignore)]
        public long ActivityId { get; set; }
        [JsonProperty("availableDates", NullValueHandling = NullValueHandling.Ignore)]
        public List<AvailableDayAndHours> AvailableDates { get; set; }
    }
}