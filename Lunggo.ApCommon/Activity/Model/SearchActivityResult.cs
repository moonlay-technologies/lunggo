using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Activity.Model
{
    public class SearchActivityResult
    {
        [JsonProperty("searchId", NullValueHandling = NullValueHandling.Ignore)]
        public int SearchId { get; set; } 
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
        [JsonProperty("activities", NullValueHandling = NullValueHandling.Ignore)]
        public List<ActivityDetail> Activities { get; set; }
        [JsonProperty("price", NullValueHandling = NullValueHandling.Ignore)]
        public decimal Price{ get; set; } 
        //[JsonProperty("hotels", NullValueHandling = NullValueHandling.Ignore)]
        //public List<HotelDetail> HotelDetails { get; set; }
        //[JsonProperty("Room", NullValueHandling = NullValueHandling.Ignore)]
        //public HotelRoomForDisplay Room { get; set; }
        //[JsonProperty("maxPrice", NullValueHandling = NullValueHandling.Ignore)]
        //public decimal MaxPrice { get; set; }
        //[JsonProperty("minPrice", NullValueHandling = NullValueHandling.Ignore)]
        //public decimal MinPrice { get; set; }
        //[JsonProperty("destinationName", NullValueHandling = NullValueHandling.Ignore)]
        //public string DestinationName { get; set; }
        //[JsonProperty("hotelFilterDisplayInfo", NullValueHandling = NullValueHandling.Ignore)]
        //public HotelFilterDisplayInfo HotelFilterDisplayInfo { get; set; }
        //[JsonProperty("occupancies", NullValueHandling = NullValueHandling.Ignore)]
        //public List<Occupancy> Occupancies { get; set; } 
    }
}
