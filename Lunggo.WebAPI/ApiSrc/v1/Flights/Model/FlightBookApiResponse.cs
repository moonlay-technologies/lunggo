using System;
using System.Net;
using Lunggo.ApCommon.Flight.Constant;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Model
{
    public class FlightBookApiResponse
    {
        public string RsvNo { get; set; }
        public string PaymentUrl { get; set; }
        public DateTime? TimeLimit { get; set; }
        [JsonProperty("status_code")]
        public HttpStatusCode StatusCode { get; set; }
        [JsonProperty("status_message")]
        public string StatusMessage { get; set; }
        public FlightBookApiRequest OriginalRequest { get; set; }
    }
}