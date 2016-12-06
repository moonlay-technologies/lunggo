using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Hotel.Model
{
    public class SearchHotelResult
    {
        [JsonProperty("searchId", NullValueHandling = NullValueHandling.Ignore)]
        public string SearchId { get; set; } 
        [JsonProperty("checkIn", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime CheckIn { get; set; }
        [JsonProperty("checkOut", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime CheckOut{ get; set; } 
        [JsonProperty("hotels", NullValueHandling = NullValueHandling.Ignore)]
        public List<HotelDetail> HotelDetails { get; set; }
        [JsonProperty("Room", NullValueHandling = NullValueHandling.Ignore)]
        public HotelRoomForDisplay Room { get; set; }
        [JsonProperty("maxPrice", NullValueHandling = NullValueHandling.Ignore)]
        public decimal MaxPrice { get; set; }
        [JsonProperty("minPrice", NullValueHandling = NullValueHandling.Ignore)]
        public decimal MinPrice { get; set; }
        [JsonProperty("destinationName", NullValueHandling = NullValueHandling.Ignore)]
        public string DestinationName { get; set; }
        [JsonProperty("hotelFilterDisplayInfo", NullValueHandling = NullValueHandling.Ignore)]
        public HotelFilterDisplayInfo HotelFilterDisplayInfo { get; set; }
        [JsonProperty("occupancies", NullValueHandling = NullValueHandling.Ignore)]
        public List<Occupancy> Occupancies { get; set; } 
    }
}
