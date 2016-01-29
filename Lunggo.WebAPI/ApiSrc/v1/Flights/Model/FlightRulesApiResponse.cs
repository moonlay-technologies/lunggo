using System.Collections.Generic;
using System.Net;
using Lunggo.ApCommon.Flight.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Model
{
    public class FlightRulesApiResponse
    {
        [JsonProperty("status_code")]
        public HttpStatusCode StatusCode { get; set; }
        [JsonProperty("status_message")]
        public string StatusMessage { get; set; }
        [JsonProperty("airline_rules")]
        public List<AirlineRules> AirlineRules { get; set; }
        [JsonProperty("baggage_rules")]
        public List<BaggageRules> BaggageRules { get; set; }
        [JsonProperty("original_request")]
        public FlightRulesApiRequest OriginalRequest { get; set; }
    }
}