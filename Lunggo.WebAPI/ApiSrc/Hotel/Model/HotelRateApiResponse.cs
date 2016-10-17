using System;
using System.Collections.Generic;
using Lunggo.ApCommon.Hotel.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Hotel.Model
{
    public class HotelRateApiResponse
    {
        [JsonProperty("rates")]
        public List<HotelRate> Rates { get; set; } 
    }
}