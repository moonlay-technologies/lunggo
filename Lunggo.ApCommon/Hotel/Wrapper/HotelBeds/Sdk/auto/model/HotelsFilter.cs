using System.Collections.Generic;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.common;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.util;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.model
{
    public class HotelsFilter
    {
        public List<int> hotel { get; set; }
        [JsonProperty("included", Required = Required.Always)]
        [JsonConverter(typeof(JSonConverters.BooleanConverter))]
        public bool included { get; set; }
        [JsonProperty("type", Required = Required.Always)]
        [JsonConverter(typeof(JSonConverters.HotelCodeTypeConverter))]
        public SimpleTypes.HotelCodeType? type { get; set; }        
    }
}
