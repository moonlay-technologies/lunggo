using System;
using Lunggo.ApCommon.Hotel.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Hotel.Model
{
    public class HotelSearchApiRequest
    {
        [JsonProperty("searchId")]
        public string SearchId { get; set; }
        [JsonProperty("location")]
        public long Location { get; set; }
        [JsonProperty("checkinDate")]
        public DateTime CheckinDate { get; set; }
        [JsonProperty("checkoutDate")]
        public DateTime CheckoutDate { get; set; }
        [JsonProperty("adultCount")]
        public int AdultCount { get; set; }
        [JsonProperty("childCount")]
        public int ChildCount { get; set; }
        [JsonProperty("nightCount")]
        public int NightCount { get; set; }
        [JsonProperty("from")]
        public int From { get; set; }
        [JsonProperty("to")]
        public int To { get; set; }
        [JsonProperty("roomCount")]
        public int RoomCount { get; set; }
        [JsonProperty("hotelFilter")]
        public HotelFilter Filter { get; set; }
        [JsonProperty("hotelSorting")]
        public string Sorting { get; set; }
    }
}
