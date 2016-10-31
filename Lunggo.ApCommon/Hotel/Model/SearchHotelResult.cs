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
        [JsonProperty("hotels", NullValueHandling = NullValueHandling.Ignore)]
        public List<HotelDetail> HotelDetails { get; set; }
        [JsonProperty("hotelFilterDisplayInfo", NullValueHandling = NullValueHandling.Ignore)]
        public HotelFilterDisplayInfo HotelFilterDisplayInfo { get; set; }
    }
}
