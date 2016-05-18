using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Flight.Model
{
    public class FlightRulesApiRequest
    {
        [JsonProperty("searchId")]
        public string SearchId { get; set; }
        [JsonProperty("reg")]
        public int ItinIndex { get; set; }
    }
}