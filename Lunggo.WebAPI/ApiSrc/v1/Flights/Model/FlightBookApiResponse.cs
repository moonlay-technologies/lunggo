using System;
using System.Net;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.WebAPI.ApiSrc.v1.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Model
{
    public class FlightBookApiResponse : ApiResponseBase
    {
        [JsonProperty("rsvno")]
        public string RsvNo { get; set; }
        [JsonProperty("url")]
        public string PaymentUrl { get; set; }
        [JsonProperty("lim")]
        public DateTime? TimeLimit { get; set; }
    }
}