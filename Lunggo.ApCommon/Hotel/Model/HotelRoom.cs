using System;
using System.Collections.Generic;
using Lunggo.ApCommon.Model;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Hotel.Model
{

    public class HotelRoom
    {
        [JsonProperty("roomCode", NullValueHandling = NullValueHandling.Ignore)]
        public string RoomCode { get; set; }
        [JsonProperty("Type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }
        [JsonProperty("facilityCode", NullValueHandling = NullValueHandling.Ignore)]
        public List<HotelRoomFacilities> Facilities { get; set; }
        [JsonProperty("roomImages", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Images { get; set; }
        [JsonProperty("rates", NullValueHandling = NullValueHandling.Ignore)]
        public List<HotelRate> Rates { get; set; }
    }
}
