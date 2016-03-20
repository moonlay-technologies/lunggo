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
        [JsonProperty("sid")]
        public string SearchId { get; set; }
        [JsonProperty("fl")]
        public List<Flight> Flights { get; set; }
        [JsonProperty("exp", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? ExpiryTime { get; set; }
        [JsonProperty("max_req")]
        public int MaxRequest { get; set; }
        [JsonProperty("gr_req")]
        public List<int> GrantedRequests { get; set; }
    }

    public class Flight
    {
        [JsonProperty("cnt")]
        public int Count { get; set; }
        [JsonProperty("opt")]
        public List<FlightItineraryForDisplay> Itineraries { get; set; }
    }
}