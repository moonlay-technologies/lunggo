using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Model
{
    public class FlightRulesApiRequest
    {
        [JsonProperty("sid")]
        public string SearchId { get; set; }
        [JsonProperty("r")]
        public int ItinIndex { get; set; }
    }
}