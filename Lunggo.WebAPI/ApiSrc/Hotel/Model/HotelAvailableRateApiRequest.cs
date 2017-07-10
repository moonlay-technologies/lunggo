using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lunggo.ApCommon.Hotel.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Hotel.Model
{
    public class HotelAvailableRateApiRequest
    {
        [JsonProperty("searchId")]
        public string SearchId { get; set; }
        [JsonProperty("hotelCode")]
        public int HotelCode { get; set; }
        [JsonProperty("nights")]
        public int Nights { get; set; }
        [JsonProperty("checkIn")]
        public DateTime CheckIn { get; set; }
        [JsonProperty("checkout")]
        public DateTime Checkout { get; set; }
        [JsonProperty("occupancies")]
        public List<Occupancy> Occupancies { get; set; }
    }
}