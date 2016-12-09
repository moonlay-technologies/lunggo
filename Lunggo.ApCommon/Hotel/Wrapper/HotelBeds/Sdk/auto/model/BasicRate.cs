using System.Collections.Generic;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.common;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.util;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.model
{
    public class BasicRate
    {
        public string rateKey { get; set; }
        public string rateClass { get; set; }
        [JsonConverter(typeof(JSonConverters.RateTypeConverter))]
        public SimpleTypes.RateType? rateType { get; set; }
        public decimal net { get; set; }
        public decimal discount { get; set; }
        public decimal discountPCT { get; set; }
        public decimal sellingRate { get; set; }
        public decimal hotelSellingRate { get; set; }
        public decimal amount { get; set; }
        public string hotelCurrency { get; set; }
        public bool hotelMandatory { get; set; }
        public List<DailyRate> dailyRates { get; set; }
        public int allotment { get; set; }
        public decimal commission { get; set; }
        public decimal commissionVAT { get; set; }
        public decimal commissionPCT { get; set; }

    }
}
