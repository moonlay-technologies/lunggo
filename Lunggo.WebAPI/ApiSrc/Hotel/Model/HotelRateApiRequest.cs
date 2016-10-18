using System;
using System.Collections.Generic;
using Lunggo.ApCommon.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Hotel.Model
{
    public class HotelRateApiRequest
    {
        [JsonProperty("hotelCode")]
        public int HotelCode { get; set; }
        [JsonProperty("searchId")]
        public new string SearchId { get; set; }       
    } 
}
