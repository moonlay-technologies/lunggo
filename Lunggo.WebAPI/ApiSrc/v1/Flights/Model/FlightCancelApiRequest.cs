using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Model
{
    public class FlightCancelApiRequest
    {
        [JsonProperty("bid")]
        public string BookingId { get; set; }
    }
}