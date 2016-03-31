using System.Net;
using Lunggo.WebAPI.ApiSrc.v1.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Model
{
    public class FlightRevalidateApiResponse : ApiResponseBase
    {
        [JsonProperty("pr")]
        public decimal? NewFare { get; set; }
    }
}