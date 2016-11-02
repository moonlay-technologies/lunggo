using System;
using System.Collections.Generic;
using Lunggo.ApCommon.Hotel.Constant;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.ApCommon.Hotel.Model.Logic;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Hotel.Model
{
    public class HotelSearchApiRequest
    {
        [JsonProperty("searchHotelType")]
        public SearchHotelType SearchType { get; set; }
        [JsonProperty("searchId")]
        public string SearchId { get; set; }
        [JsonProperty("hotelFilter")]
        public HotelFilter Filter { get; set; }
        [JsonProperty("hotelSorting")]
        public string Sorting { get; set; }

        [JsonProperty("location")]
        public long Location { get; set; }
        [JsonProperty("checkinDate")]
        public DateTime CheckinDate { get; set; }
        [JsonProperty("checkoutDate")]
        public DateTime CheckoutDate { get; set; }
        //[JsonProperty("adultCount")]
        //public int AdultCount { get; set; }
        //[JsonProperty("childCount")]
        //public int ChildCount { get; set; }
        [JsonProperty("nightCount")]
        public int NightCount { get; set; }
        [JsonProperty("from")]
        public int From { get; set; }
        [JsonProperty("to")]
        public int To { get; set; }
        [JsonProperty("occupancies")]
        public List<Occupancy> Occupancies { get; set; } 
        public List<PaxDataInput> PaxData { get; set; }
        //[JsonProperty("roomCount")]
        //public int RoomCount { get; set; }

        [JsonProperty("HotelCd")]
        public int HotelCode { get; set; }
        [JsonProperty("RateKey")]
        public string RateKey { get; set; }

    }

}
