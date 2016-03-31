using System;
using System.Collections.Generic;
using System.Net;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.WebAPI.ApiSrc.v1.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Model
{
    public class FlightSearchApiResponse : ApiResponseBase
    {
        [JsonProperty("fl", NullValueHandling = NullValueHandling.Ignore)]
        public List<Flight> Flights { get; set; }
        [JsonProperty("cmb", NullValueHandling = NullValueHandling.Ignore)]
        public List<Combo> Combos { get; set; }
        [JsonProperty("exp", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? ExpiryTime { get; set; }
        [JsonProperty("prog", NullValueHandling = NullValueHandling.Ignore)]
        public int? Progress { get; set; }
    }

    public class Flight
    {
        [JsonProperty("cnt")]
        public int Count { get; set; }
        [JsonProperty("opt")]
        public List<FlightItineraryForDisplay> Itineraries { get; set; }
    }
}