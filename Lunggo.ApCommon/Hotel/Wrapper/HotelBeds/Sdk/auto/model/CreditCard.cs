using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.common;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.util;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.model
{
    public class CreditCard
    {
        public string code { get; set; }
        public string name { get; set; }
        [JsonProperty("paymentType", Required = Required.Always)]
        [JsonConverter(typeof(JSonConverters.PaymentTypeConverter))]
        public SimpleTypes.PaymentType paymentType { get; set; }
    }
}
