using System;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.common;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.util;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.model
{
    public class Source
    {
        [JsonProperty("channel", Required = Required.Always)]
        [JsonConverter(typeof(JSonConverters.ChannelTypeConverter))]
        public SimpleTypes.ChannelType? channel { get; set; }
        [JsonProperty("device", Required = Required.Always)]
        [JsonConverter(typeof(JSonConverters.DeviceTypeConverter))]
        public SimpleTypes.DeviceType? device { get; set; }
        public String deviceInfo { get; set; }
        
    }
}
