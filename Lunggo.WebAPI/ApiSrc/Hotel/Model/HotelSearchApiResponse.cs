using System;
using System.Collections.Generic;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Hotel.Model
{
    public class HotelSearchApiResponse : ApiResponseBase
    {
        [JsonProperty("searchId", NullValueHandling = NullValueHandling.Ignore)]
        public string SearchId { get; set; }
        [JsonProperty("totalDisplayHotel", NullValueHandling = NullValueHandling.Ignore)]
        public int TotalDisplayHotel { get; set; }
        [JsonProperty("totalActualHotel", NullValueHandling = NullValueHandling.Ignore)]
        public int TotalActualHotel { get; set; }
        [JsonProperty("hotels", NullValueHandling = NullValueHandling.Ignore)]
        public List<HotelDetailForDisplay> Hotels { get; set; }
        [JsonProperty("expTime", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? ExpiryTime { get; set; }
        [JsonProperty("from", NullValueHandling = NullValueHandling.Ignore)]
        public int From { get; set; }
        [JsonProperty("to", NullValueHandling = NullValueHandling.Ignore)]
        public int To { get; set; }
        [JsonProperty("hotelFilterDisplayInfo", NullValueHandling = NullValueHandling.Ignore)]
        public HotelFilterDisplayInfo HotelFilterDisplayInfo { get; set; }

    }
}