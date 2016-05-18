using System;
using System.Collections.Generic;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.WebAPI.ApiSrc.Common.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.Flight.Model
{
    public class FlightSearchApiResponse : ApiResponseBase
    {
        [JsonProperty("flights", NullValueHandling = NullValueHandling.Ignore)]
        public List<Flight> Flights { get; set; }
        [JsonProperty("combos", NullValueHandling = NullValueHandling.Ignore)]
        public List<ComboForDisplay> Combos { get; set; }
        [JsonProperty("expTime", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? ExpiryTime { get; set; }
        [JsonProperty("progress", NullValueHandling = NullValueHandling.Ignore)]
        public int? Progress { get; set; }
    }

    public class Flight
    {
        [JsonProperty("count")]
        public int Count { get; set; }
        [JsonProperty("options")]
        public List<FlightItineraryForDisplay> Itineraries { get; set; }
    }
}