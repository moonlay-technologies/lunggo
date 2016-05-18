using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Flight.Model
{
    public class FlightRevalidateApiRequest
    {
        [JsonProperty("token")]
        public string Token { get; set; }
    }
}