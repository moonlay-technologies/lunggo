using Lunggo.WebAPI.ApiSrc.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Auxiliary.Model
{
    public class DetailPromoApiResponse : ApiResponseBase
    {
        [JsonProperty("detailPromo")]
        public DetailPromo DetailPromo { get; set; }
    }

    public class DetailPromo
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("tnc")]
        public string[] Tnc { get; set; }
        [JsonProperty("bannerUrl")]
        public string BannerUrl { get; set; }
        [JsonProperty("hotelPromos")]
        public Hotels Hotels { get; set; }
        [JsonProperty("flightPromos")]
        public Flights Flights { get; set; }
        [JsonProperty("bookingPeriod")]
        public string BookingPeriod { get; set; }
        [JsonProperty("travelPeriod")]
        public string TravelPeriod { get; set; }
    }
}