using System;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Flight.Model
{
    public class FlightSelectApiResponse : ApiResponseBase
    {
        [JsonProperty("token", NullValueHandling = NullValueHandling.Ignore)]
        public string Token { get; set; }
        [JsonProperty("expTime", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? ExpiryTime { get; set; }
    }
}