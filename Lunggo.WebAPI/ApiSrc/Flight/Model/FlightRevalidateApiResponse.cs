using Lunggo.ApCommon.Flight.Model;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Flight.Model
{
    public class FlightRevalidateApiResponse : ApiResponseBase
    {
        [JsonProperty("valid", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsValid { get; set; }
        [JsonProperty("itinChanged", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsItineraryChanged { get; set; }
        [JsonProperty("priceChanged", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsPriceChanged { get; set; }
        [JsonProperty("itin", NullValueHandling = NullValueHandling.Ignore)]
        public FlightItineraryForDisplay NewItinerary { get; set; }
        [JsonProperty("price", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? NewPrice { get; set; }
    }
}