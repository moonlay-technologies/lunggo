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
        [JsonProperty("customHour", NullValueHandling = NullValueHandling.Ignore)]
        public string CustomHour { get; set; }
        [JsonProperty("dateStatus", NullValueHandling = NullValueHandling.Ignore)]
        public string DateStatus { get; set; }
    }
}