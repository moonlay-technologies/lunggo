using Lunggo.ApCommon.Model;
using Newtonsoft.Json;

namespace Lunggo.ApCommon.Flight.Model
{
    public class TopDestination
    {
        [JsonProperty("ori")]
        public string OriginCity { get; set; }
        [JsonProperty("des")]
        public string DestinationCity { get; set; }
        [JsonProperty("price")]
        public Price CheapestPrice { get; set; }
    }
}
