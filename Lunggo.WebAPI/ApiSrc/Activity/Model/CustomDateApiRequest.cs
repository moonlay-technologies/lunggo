using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lunggo.WebAPI.ApiSrc.Activity.Model
{
    public class CustomDateApiRequest
    {
        [JsonProperty("activityId", NullValueHandling = NullValueHandling.Ignore)]
        public long ActivityId { get; set; }
        [JsonProperty("customDate", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime CustomDate { get; set; }
        [JsonProperty("startCustomHour", NullValueHandling = NullValueHandling.Ignore)]
        public string StartCustomHour { get; set; }
        [JsonProperty("endCustomHour", NullValueHandling = NullValueHandling.Ignore)]
        public string EndCustomHour { get; set; }
    }
}