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
        [JsonProperty("hotelPromo")]
        public Hotels Hotels { get; set; }
        [JsonProperty("flightPromo")]
        public Flights Flights { get; set; }
    }
}