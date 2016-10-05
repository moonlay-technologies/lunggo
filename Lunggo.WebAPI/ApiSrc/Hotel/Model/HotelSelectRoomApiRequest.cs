using System;
using System.Collections.Generic;
using Lunggo.ApCommon.Hotel.Object;
using Lunggo.ApCommon.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Hotel.Model
{
    public class HotelSelectRoomApiRequest
    {
        [JsonProperty("rateKey")]
        public string RateKey { get; set; }    
    } 
}
