using System;
using System.Collections.Generic;
using Lunggo.ApCommon.Hotel.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Hotel.Model
{
    public class SelectRoomApiResponse
    {
        [JsonProperty("token")]
        public string Token{ get; set; } 
        [JsonProperty("timeLimit")]
        public DateTime TimeLimit { get; set; }
    }
}