using System;
using System.Collections.Generic;
using Lunggo.ApCommon.Hotel.Constant;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Hotel.Model
{
    public class HotelSearchApiResponse : ApiResponseBase
    {
        [JsonProperty("searchId", NullValueHandling = NullValueHandling.Ignore)]
        public string SearchId { get; set; }
        [JsonProperty("IsSpecificHotel", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsSpecificHotel { get; set; }
        [JsonProperty("hotelCd", NullValueHandling = NullValueHandling.Ignore)]
        public int? HotelCode { get; set; }
        [JsonProperty("returnedHotelCount", NullValueHandling = NullValueHandling.Ignore)]
        public int? ReturnedHotelCount { get; set; }
        [JsonProperty("totalHotelCount", NullValueHandling = NullValueHandling.Ignore)]
        public int? TotalHotelCount { get; set; }
        [JsonProperty("filteredHotelCount", NullValueHandling = NullValueHandling.Ignore)]
        public int? FilteredHotelCount { get; set; }
        [JsonProperty("hotels", NullValueHandling = NullValueHandling.Ignore)]
        public List<HotelDetailForDisplay> Hotels { get; set; }
        [JsonProperty("room", NullValueHandling = NullValueHandling.Ignore)]
        public HotelRoomForDisplay Room { get; set; }
        [JsonProperty("expTime", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? ExpiryTime { get; set; }
        [JsonProperty("from", NullValueHandling = NullValueHandling.Ignore)]
        public int? From { get; set; }
        [JsonProperty("to", NullValueHandling = NullValueHandling.Ignore)]
        public int? To { get; set; }
        [JsonProperty("maxPrice", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? MaxPrice { get; set; }
        [JsonProperty("minPrice", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? MinPrice { get; set; }
        [JsonProperty("hotelFilterDisplayInfo", NullValueHandling = NullValueHandling.Ignore)]
        public HotelFilterDisplayInfo HotelFilterDisplayInfo { get; set; }
        [JsonProperty("page", NullValueHandling = NullValueHandling.Ignore)]
        public int? Page { get; set; }
        [JsonProperty("perPage", NullValueHandling = NullValueHandling.Ignore)]
        public int? PerPage { get; set; }

    }
}