using System;
using System.Collections.Generic;
using Lunggo.ApCommon.Hotel.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Hotel.Model
{
    public class HotelRoomDetailApiResponse
    {
        [JsonProperty("roomDetails")]
        public List<HotelRoom> RoomDetails { get; set; } 
    }
}