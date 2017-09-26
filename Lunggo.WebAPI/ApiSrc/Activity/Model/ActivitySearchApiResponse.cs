using Lunggo.WebAPI.ApiSrc.Common.Model;
using System.Collections.Generic;
using Lunggo.ApCommon.Activity.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Activity.Model
{
    public class ActivitySearchApiResponse : ApiResponseBase
    {
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
        [JsonProperty("ActivityList", NullValueHandling = NullValueHandling.Ignore)]
        public List<ActivityDetail> ActivityList { get; set; }
        //[JsonProperty("totalCount", NullValueHandling = NullValueHandling.Ignore)]
        //public int? TotalCount { get; set; }
        //[JsonProperty("activities", NullValueHandling = NullValueHandling.Ignore)]
        //public List<HotelDetailForDisplay> Activities { get; set; }
        //[JsonProperty("from", NullValueHandling = NullValueHandling.Ignore)]
        //public int? From { get; set; }
        //[JsonProperty("to", NullValueHandling = NullValueHandling.Ignore)]
        //public int? To { get; set; }
        //[JsonProperty("maxPrice", NullValueHandling = NullValueHandling.Ignore)]
        //public decimal? MaxPrice { get; set; }
        //[JsonProperty("minPrice", NullValueHandling = NullValueHandling.Ignore)]
        //public decimal? MinPrice { get; set; }
        //[JsonProperty("filter", NullValueHandling = NullValueHandling.Ignore)]
        //public ActivityFilterDisplayInfo HotelFilterDisplayInfo { get; set; }
        //[JsonProperty("page", NullValueHandling = NullValueHandling.Ignore)]
        //public int? Page { get; set; }
        //[JsonProperty("perPage", NullValueHandling = NullValueHandling.Ignore)]
        //public int? PerPage { get; set; }
        //[JsonProperty("pageCount", NullValueHandling = NullValueHandling.Ignore)]
        //public int? PageCount { get; set; }
    }
}