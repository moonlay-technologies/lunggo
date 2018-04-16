using Lunggo.ApCommon.Activity.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lunggo.WebAPI.ApiSrc.Activity.Model
{
    public class InsertActivityRatingApiRequest
    {
        [JsonProperty("answers", NullValueHandling = NullValueHandling.Ignore)]
        public List<ActivityRatingAnswer> Answers { get; set; }
        [JsonProperty("rsvNo", NullValueHandling = NullValueHandling.Ignore)]
        public string RsvNo { get; set; }
    }
}