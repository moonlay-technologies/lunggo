using System.Collections.Generic;
using Lunggo.ApCommon.Hotel.Model;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Hotel.Model
{
    public class HotelRateApiResponse : ApiResponseBase
    {
        [JsonProperty("rooms")]
        public List<HotelRoomForDisplay> Rooms { get; set; }
        [JsonProperty("searchId")]
        public string SearchId { get; set; } 
    }
}