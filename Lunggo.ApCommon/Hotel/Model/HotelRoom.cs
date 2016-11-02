using System;
using System.Collections.Generic;
using Lunggo.ApCommon.Model;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Hotel.Model
{

    public class HotelRoomForDisplay
    {
        [JsonProperty("roomCode", NullValueHandling = NullValueHandling.Ignore)]
        public string RoomCode { get; set; }
        [JsonProperty("roomName", NullValueHandling = NullValueHandling.Ignore)]
        public string RoomName { get; set; }
        [JsonProperty("Type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }
        [JsonProperty("TypeName", NullValueHandling = NullValueHandling.Ignore)]
        public string TypeName { get; set; }
        [JsonProperty("facilityCode", NullValueHandling = NullValueHandling.Ignore)]
        public List<HotelRoomFacilities> Facilities { get; set; }
        [JsonProperty("roomImages", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Images { get; set; }
        [JsonProperty("rates", NullValueHandling = NullValueHandling.Ignore)]
        public List<HotelRateForDisplay> Rates { get; set; }
        [JsonProperty("characteristic", NullValueHandling = NullValueHandling.Ignore)]
        public string CharacteristicCode { get; set; }
        [JsonProperty("characteristicName", NullValueHandling = NullValueHandling.Ignore)]
        public string CharacteristicName { get; set; }
        [JsonProperty("paxCapacity", NullValueHandling = NullValueHandling.Ignore)]
        public int PaxCapacity { get; set; }
        [JsonProperty("rate", NullValueHandling = NullValueHandling.Ignore)]
        public HotelRateForDisplay SingleRate { get; set; }

    }
    
    public class HotelRoom
    {
        [JsonProperty("roomCode", NullValueHandling = NullValueHandling.Ignore)]
        public string RoomCode { get; set; }
        [JsonProperty("roomName", NullValueHandling = NullValueHandling.Ignore)]
        public string RoomName { get; set; }
        [JsonProperty("Type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }
        [JsonProperty("TypeName", NullValueHandling = NullValueHandling.Ignore)]
        public string TypeName { get; set; }
        [JsonProperty("facilityCode", NullValueHandling = NullValueHandling.Ignore)]
        public List<HotelRoomFacilities> Facilities { get; set; }
        [JsonProperty("roomImages", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Images { get; set; }
        [JsonProperty("rates", NullValueHandling = NullValueHandling.Ignore)]
        public List<HotelRate> Rates { get; set; }
        [JsonProperty("rate", NullValueHandling = NullValueHandling.Ignore)]
        public HotelRate SingleRate { get; set; }
        [JsonProperty("characteristic", NullValueHandling = NullValueHandling.Ignore)]
        public string characteristicCd { get; set; }
    }
}
