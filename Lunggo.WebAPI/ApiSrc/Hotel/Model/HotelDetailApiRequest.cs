using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Hotel.Model
{
    public class HotelDetailApiRequest
    {
        [JsonProperty("hotelCode")]
        public string HotelCode { get; set; }
        [JsonProperty("searchId")]
        public new string SearchId { get; set; }
    }
  
}
