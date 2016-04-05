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
        [JsonProperty("iss", NullValueHandling = NullValueHandling.Ignore)]
        public List<TripIssuance> TripIssuances { get; set; }
    }

    public class TripIssuance
    {
        [JsonProperty("ori")]
        public string Origin { get; set; }
        [JsonProperty("des")]
        public string Destination { get; set; }
        [JsonProperty("dt")]
        public DateTime Date { get; set; }
        [JsonProperty("st")]
        public int Status { get; set; }
    }
}