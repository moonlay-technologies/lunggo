using System;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Flight.Model
{
    public class FlightBookApiResponse : FlightRevalidateApiResponse
    {
        [JsonProperty("rsvNo", NullValueHandling = NullValueHandling.Ignore)]
        public string RsvNo { get; set; }
        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public string PaymentUrl { get; set; }
        [JsonProperty("timeLimit", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? TimeLimit { get; set; }
    }
}