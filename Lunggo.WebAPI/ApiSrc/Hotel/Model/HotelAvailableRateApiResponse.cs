using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Hotel.Model
{
    public class HotelAvailableRateApiResponse : ApiResponseBase
    {
        [JsonProperty("rooms")]
        public List<HotelRoomForDisplay> Rooms { get; set; }
        [JsonProperty("total")]
        public int Total { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}