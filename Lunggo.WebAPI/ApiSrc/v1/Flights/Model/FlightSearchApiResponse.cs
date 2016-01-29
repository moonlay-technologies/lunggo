using System;
using System.Collections.Generic;
using System.Net;
using Lunggo.ApCommon.Flight.Model;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Model
{
    public class FlightSearchApiResponse
    {
        [JsonProperty("search_id")]
        public string SearchId { get; set; }
        [JsonProperty("flights_count")]
        public int TotalFlightCount { get; set; }
        [JsonProperty("flights")]
        public List<FlightItineraryForDisplay> FlightList { get; set; }
        [JsonProperty("expiry_time")]
        public DateTime? ExpiryTime { get; set; }
        [JsonProperty("max_requests")]
        public int MaxRequest { get; set; }
        [JsonProperty("granted_requests")]
        public List<int> GrantedRequests { get; set; }
        [JsonProperty("status_code")]
        public HttpStatusCode StatusCode { get; set; }
        [JsonProperty("status_message")]
        public string StatusMessage { get; set; }
        [JsonProperty("original_request")]
        public FlightSearchApiRequest OriginalRequest { get; set; }   
    }
}