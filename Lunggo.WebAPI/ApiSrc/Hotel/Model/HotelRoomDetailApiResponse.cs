using System;
using System.Collections.Generic;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Hotel.Model
{
    public class HotelRoomDetailApiResponse : ApiResponseBase
    {
        [JsonProperty("roomDetails", NullValueHandling = NullValueHandling.Ignore)]
        public HotelRoomForDisplay RoomDetails { get; set; } 
    }
}