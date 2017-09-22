using System;
using System.Collections.Generic;
//using Lunggo.ApCommon.Hotel.Constant;
//using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Activity.Model;
using Lunggo.ApCommon.Hotel.Model.Logic;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Activity.Model
{
    public class ActivitySearchApiRequest
    {
        [JsonProperty("filter")]
        public ActivityFilter Filter { get; set; }
        [JsonProperty("sorting")]
        public string Sorting { get; set; }
        [JsonProperty("country")]
        public string Country { get; set; }
        [JsonProperty("date")]
        public DateTime Date { get; set; }
        [JsonProperty("page")]
        public int Page { get; set; }
        [JsonProperty("perPage")]
        public int PerPage { get; set; }
        [JsonProperty("pageCount")]
        public int PageCount { get; set; }
    }
}
