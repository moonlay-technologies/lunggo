using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.model
{
    public class Occupancy
    {
        public int? rooms { get; set; }
        [JsonProperty("adults", Required = Required.Always)]
        public int? adults { get; set; }
        [DefaultValue(30)]
        public int? children { get; set; }
        public List<Pax> paxes { get; set; }
    }
}
