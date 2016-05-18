using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Flight.Model
{
    public class FlightSearchApiRequest
    {
        [JsonProperty("searchId")]
        public string SearchId { get; set; }
        [JsonProperty("progress")]
        public int Progress { get; set; }
    }
}