using System;
using System.Net;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.WebAPI.ApiSrc.v1.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Model
{
    public class FlightBookApiResponse : ApiResponseBase
    {
        [JsonProperty("rsvno", NullValueHandling = NullValueHandling.Ignore)]
        public string RsvNo { get; set; }
        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public string PaymentUrl { get; set; }
        [JsonProperty("lim", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? TimeLimit { get; set; }
    }
}