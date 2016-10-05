using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.common;
using Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.util;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Sdk.auto.model
{
    public class ReviewFilter
    {
        [JsonProperty("type", Required = Required.Always)]
        [JsonConverter(typeof(JSonConverters.AccommodationTypeConverter))]
        public SimpleTypes.ReviewsType? type { get; set; }
        public decimal minRate { get; set; }
        public decimal maxRate { get; set; }
        public int minReviewCount { get; set; }
    }
}
