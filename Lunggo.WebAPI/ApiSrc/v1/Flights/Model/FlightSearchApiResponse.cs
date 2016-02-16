﻿using System;
using System.Collections.Generic;
using Lunggo.ApCommon.Flight.Model;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Model
{
    public class FlightSearchApiResponse
    {
        public string SearchId { get; set; }
        public int TotalFlightCount { get; set; }
        public List<FlightItineraryForDisplay> FlightList { get; set; }
        public int TotalReturnFlightCount { get; set; }
        public List<FlightItineraryForDisplay> ReturnFlightList { get; set; }
        public DateTime? ExpiryTime { get; set; }
        public int MaxRequest { get; set; }
        public List<int> GrantedRequests { get; set; }
        public FlightSearchApiRequest OriginalRequest { get; set; }   
    }
}