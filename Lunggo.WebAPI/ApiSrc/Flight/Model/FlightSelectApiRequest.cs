using System.Collections.Generic;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Flight.Model
{
    public class FlightSelectApiRequest
    {
        [JsonProperty("searchId")]
        public string SearchId { get; set; }
        [JsonProperty("regs")]
        public List<int> RegisterNumbers { get; set; }
        [JsonProperty("enableCombo")]
        public bool EnableCombo { get; set; }
    }
}