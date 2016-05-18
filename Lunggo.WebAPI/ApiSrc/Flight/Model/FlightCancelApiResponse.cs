using Lunggo.WebAPI.ApiSrc.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Flight.Model
{
    public class FlightCancelApiResponse : ApiResponseBase
    {
        [JsonProperty("bid")]
        public string BookingId { get; set; }
    }
}