using System.Net;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.WebAPI.ApiSrc.v1.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Model
{
    public class FlightRevalidateApiResponse : ApiResponseBase
    {
        [JsonProperty("vld", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsValid { get; set; }
        [JsonProperty("itinchgd", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsItineraryChanged { get; set; }
        [JsonProperty("prchgd", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsPriceChanged { get; set; }
        [JsonProperty("itin", NullValueHandling = NullValueHandling.Ignore)]
        public FlightItineraryForDisplay NewItinerary { get; set; }
        [JsonProperty("pr", NullValueHandling = NullValueHandling.Ignore)]
        public decimal? NewPrice { get; set; }
    }
}