using System;
using System.Collections.Generic;
using Lunggo.ApCommon.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Hotel.Model
{
    public class HotelRoomDetailApiRequest
    {
        [JsonProperty("hotelCode")]
        public string HotelCode { get; set; }
        [JsonProperty("roomCodes")]
        public List<string> RoomCodes { get; set; }       
    } 
}
