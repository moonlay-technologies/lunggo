using System;
using System.Collections.Generic;
using Lunggo.ApCommon.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Hotel.Model
{
    public class HotelSelectedRoomApiRequest
    {
        [JsonProperty("token")]
        public string Token { get; set; }
    } 
}
