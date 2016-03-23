using System.Net;
using Lunggo.WebAPI.ApiSrc.v1.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Model
{
    public class FlightRevalidateApiResponse : ApiResponseBase
    {
        [JsonProperty("vld")]
        public bool IsValid { get; set; }
        [JsonProperty("avail")]
        public bool? IsOtherFareAvailable { get; set; }
        [JsonProperty("fare")]
        public decimal? NewFare { get; set; }
    }
}