using System;
using System.Collections.Generic;
//using Lunggo.ApCommon.Hotel.Constant;
//using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Model.Logic;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Activity.Model
{
    public class ActivitySearchApiRequest
    {
        [JsonProperty("searchActivityType")]
        public SearchHotelType SearchType { get; set; }
        [JsonProperty("searchId")]
        public string SearchId { get; set; }
        [JsonProperty("hotelFilter")]
        public HotelFilter Filter { get; set; }
        [JsonProperty("hotelSorting")]
        public string Sorting { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }
        [JsonProperty("checkinDate")]
        public DateTime CheckinDate { get; set; }
        [JsonProperty("nightCount")]
        public int NightCount { get; set; }
        [JsonProperty("hotelCd")]
        public int HotelCode { get; set; }
        [JsonProperty("regsId")]
        public string RegsId { get; set; }
        [JsonProperty("page")]
        public int Page { get; set; }
        [JsonProperty("perPage")]
        public int PerPage { get; set; }
        [JsonProperty("pageCount")]
        public int PageCount { get; set; }
    }
}
