using System.Collections.Generic;
using Lunggo.WebAPI.ApiSrc.v1.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Model
{
    public class FlightSelectApiResponse : ApiResponseBase
    {
        [JsonProperty("tkn")]
        public string Token { get; set; }
    }
}