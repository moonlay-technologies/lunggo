using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.common;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.util;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.model
{
    public class Filter
    {
        public int maxHotels { get; set; }
        public int maxRooms { get; set; }
        public decimal minRate { get; set; }
        public decimal maxRate { get; set; }
        public int maxRatesPerRoom { get; set; }
        public bool packaging { get; set; }
        [JsonProperty("paymentType", Required = Required.Always)]
        [JsonConverter(typeof(JSonConverters.ShowDirectPaymentTypeConverter))]
        public SimpleTypes.ShowDirectPaymentType? paymentType { get; set; }
        [JsonProperty("hotelPackage", Required = Required.Always)]
        [JsonConverter(typeof(JSonConverters.HotelPackageConverter))]
        public SimpleTypes.HotelPackage? hotelPackage { get; set; }
        public int minCategory { get; set; }
        public int maxCategory { get; set; }
    }
}
