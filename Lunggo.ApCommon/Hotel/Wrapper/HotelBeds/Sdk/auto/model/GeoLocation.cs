using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.util;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.model
{
    public class GeoLocation
    {
        public double? longitude { get; set; }
        public double? latitude { get; set; }
        public decimal? radius { get; set; }
        [JsonProperty("unit", Required = Required.Always)]
        [JsonConverter(typeof(UnitMeasure.typeConverter))]
        public UnitMeasure.UnitMeasureType? unit { get; set; }
        public double secondaryLatitude { get; set; }
        public double secondaryLongitude { get; set; }
    }
}
