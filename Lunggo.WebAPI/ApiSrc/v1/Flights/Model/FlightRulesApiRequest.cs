using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Model
{
    public class FlightRulesApiRequest
    {
        [JsonProperty("search_id")]
        public string SearchId { get; set; }
        [JsonProperty("itin_index")]
        public int ItinIndex { get; set; }
    }
}