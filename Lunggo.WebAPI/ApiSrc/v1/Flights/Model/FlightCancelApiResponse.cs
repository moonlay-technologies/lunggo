﻿using System.Net;
using Newtonsoft.Json;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Model
{
    public class FlightCancelApiResponse
    {
        [JsonProperty("status_code")]
        public HttpStatusCode StatusCode { get; set; }
        [JsonProperty("status_message")]
        public string StatusMessage { get; set; }
        [JsonProperty("booking_id")]
        public string BookingId { get; set; }
        [JsonProperty("original_request")]
        public FlightCancelApiRequest OriginalRequest { get; set; }
    }
}