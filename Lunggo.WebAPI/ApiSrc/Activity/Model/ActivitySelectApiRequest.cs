using System;
using System.Collections.Generic;
using Lunggo.ApCommon.Activity.Constant;
using Lunggo.ApCommon.Activity.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Activity.Model
{
    public class ActivitySelectApiRequest
    {
        [JsonProperty("activityId")]
        public int ActivityId { get; set; }
        //[JsonProperty("searchId")]
        //public string SearchId { get; set; }
        //[JsonProperty("activityFilter")]
        //public ActivityFilter Filter { get; set; }
        //[JsonProperty("name")]
        //public string Name { get; set; }
        //[JsonProperty("page")]
        //public int Page { get; set; }
        //[JsonProperty("perPage")]
        //public int PerPage { get; set; }
    }
}
