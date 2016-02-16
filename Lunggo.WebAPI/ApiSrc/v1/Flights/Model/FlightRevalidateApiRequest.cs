using System.Collections.Generic;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Model
{
    public class FlightRevalidateApiRequest
    {
        [JsonProperty("search_id")]
        public string SearchId { get; set; }
        [JsonProperty("registers")]
        public List<int> ItinIndices { get; set; }
        [JsonProperty("secure_code")]
        public string SecureCode { get; set; }
    }
}