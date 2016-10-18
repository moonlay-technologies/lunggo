using System;
using System.Collections.Generic;
using Lunggo.ApCommon.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Hotel.Model
{
    public class HotelRoomDetailApiRequest
    {
        [JsonProperty("hotelCode")]
        public int HotelCode { get; set; }
        [JsonProperty("roomCode")]
        public string RoomCode { get; set; }      
        [JsonProperty("searchId")]
        public string SearchId { get; set; }
    } 
}
