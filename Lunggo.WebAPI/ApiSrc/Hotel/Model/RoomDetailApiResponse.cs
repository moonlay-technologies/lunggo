using System;
using System.Collections.Generic;
using Lunggo.ApCommon.Hotel.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Hotel.Model
{
    public class RoomDetailApiResponse
    {
        [JsonProperty("roomDetails")]
        public List<RoomDetails> RoomDetails{ get; set; } 
    }
}