using System;
using System.Net;
using Lunggo.ApCommon.Flight.Constant;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Model
{
    public class FlightBookApiResponse
    {
        [JsonProperty("rsv_no")]
        public string RsvNo { get; set; }
        [JsonProperty("url")]
        public string PaymentUrl { get; set; }
        [JsonProperty("time_limit")]
        public DateTime? TimeLimit { get; set; }
        [JsonProperty("status_code")]
        public HttpStatusCode StatusCode { get; set; }
        [JsonProperty("status_message")]
        public string StatusMessage { get; set; }
        [JsonProperty("request")]
        public FlightBookApiRequest OriginalRequest { get; set; }
    }
}