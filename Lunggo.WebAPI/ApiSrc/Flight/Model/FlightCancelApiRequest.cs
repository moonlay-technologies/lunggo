using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Flight.Model
{
    public class FlightCancelApiRequest
    {
        [JsonProperty("bid")]
        public string BookingId { get; set; }
    }
}