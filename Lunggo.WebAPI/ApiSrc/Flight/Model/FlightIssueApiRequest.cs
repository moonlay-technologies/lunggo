using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Flight.Model
{
    public class FlightIssueApiRequest
    {
        [JsonProperty("rsvNo")]
        public string RsvNo { get; set; }
    }
}