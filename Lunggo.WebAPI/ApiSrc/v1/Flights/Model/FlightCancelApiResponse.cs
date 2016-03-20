using System.Net;
using Lunggo.WebAPI.ApiSrc.v1.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Model
{
    public class FlightCancelApiResponse : ApiResponseBase
    {
        [JsonProperty("bid")]
        public string BookingId { get; set; }
    }
}