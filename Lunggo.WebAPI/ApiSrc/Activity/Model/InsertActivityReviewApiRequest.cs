using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lunggo.WebAPI.ApiSrc.Activity.Model
{
    public class InsertActivityReviewApiRequest
    {
        [JsonProperty("review", NullValueHandling = NullValueHandling.Ignore)]
        public string Review { get; set; }
        [JsonProperty("date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime Date { get; set; }
        [JsonProperty("rsvNo", NullValueHandling = NullValueHandling.Ignore)]
        public string RsvNo { get; set; }
    }
}