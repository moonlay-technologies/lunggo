using System.Net;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Model
{
    public class FlightRevalidateApiResponse
    {
        [JsonProperty("token")]
        public string Token { get; set; }
        [JsonProperty("is_valid")]
        public bool IsValid { get; set; }
        [JsonProperty("is_other_fare_available")]
        public bool? IsOtherFareAvailable { get; set; }
        [JsonProperty("new_fare")]
        public decimal? NewFare { get; set; }
        [JsonProperty("status_code")]
        public HttpStatusCode StatusCode { get; set; }
        [JsonProperty("status_message")]
        public string StatusMessage { get; set; }
        [JsonProperty("original_request")]
        public FlightRevalidateApiRequest OriginalRequest { get; set; }
    }
}