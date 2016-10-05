using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.common;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.util;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.model
{
    public class Pax
    {
        public int? roomId { get; set; }
        [JsonProperty("type", Required = Required.Always)]
        [JsonConverter(typeof(JSonConverters.HotelbedsCustomerTypeConverter))]
        public SimpleTypes.HotelbedsCustomerType? type { get; set; }
        public int? age { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
    }
}
