using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.common;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.util;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.model
{
    public class Tax
    {
        public bool included { get; set; }
        public decimal percent { get; set; }
        public decimal amount { get; set; }
        public string currency { get; set; }
        [JsonProperty("type", Required = Required.Always)]
        [JsonConverter(typeof(JSonConverters.TaxTypeConverter))]
        public SimpleTypes.TaxType type { get; set; }
        public decimal clientAmount { get; set; }
        public string clientCurrency { get; set; }
    }
}
