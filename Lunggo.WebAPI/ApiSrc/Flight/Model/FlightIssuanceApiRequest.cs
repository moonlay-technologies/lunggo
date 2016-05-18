using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Flight.Model
{
    public class FlightIssuanceApiRequest
    {
        [JsonProperty("rsvNo")]
        public string RsvNo { get; set; }
    }
}