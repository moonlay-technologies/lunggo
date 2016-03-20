using System.Collections.Generic;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Model
{
    public class FlightRevalidateApiRequest
    {
        [JsonProperty("sid")]
        public string SearchId { get; set; }
        [JsonProperty("reg")]
        public List<int> ItinIndices { get; set; }
    }
}