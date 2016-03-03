using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Model
{
    public class FlightIssueApiRequest
    {
        [JsonProperty("rsv_no")]
        public string RsvNo { get; set; }
    }
}