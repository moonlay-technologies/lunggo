using System;
using System.Collections.Generic;
using System.Net;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.WebAPI.ApiSrc.v1.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Model
{
    public class FlightIssuanceApiResponse : ApiResponseBase
    {
        [JsonProperty("issuances", NullValueHandling = NullValueHandling.Ignore)]
        public List<TripIssuance> TripIssuances { get; set; }
    }

    public class TripIssuance
    {
        [JsonProperty("origin")]
        public string Origin { get; set; }
        [JsonProperty("destination")]
        public string Destination { get; set; }
        [JsonProperty("date")]
        public DateTime Date { get; set; }
        [JsonProperty("status")]
        public int Status { get; set; }
    }
}