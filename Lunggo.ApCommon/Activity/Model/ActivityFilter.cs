using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Activity.Model
{

    public class ActivityFilter
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("startDate")]
        public DateTime StartDate { get; set; }
        [JsonProperty("endDate")]
        public DateTime EndDate { get; set; }
        [JsonProperty("price")]
        public PriceFilter Price { get; set; }
    }

    public class PriceFilter
    {
        [JsonProperty("minPrice")]
        public decimal? MinPrice { get; set; }
        [JsonProperty("maxPrice")]
        public decimal? MaxPrice { get; set; }
    }

    //public class DateFilter
    //{
    //    [JsonProperty("startDate")]
    //    public DateTime StartDate { get; set; }
    //    [JsonProperty("endDate")]
    //    public DateTime EndDate { get; set; }
    //}
}
