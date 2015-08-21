using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lunggo.ApCommon.Flight.Model;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Model
{
    public class FlightSearchApiResponse
    {
        public string SearchId { get; set; }
        public int TotalFlightCount { get; set; }
        public List<FlightItineraryApi> FlightList { get; set; }
        public DateTime ExpiryTime { get; set; }
        public FlightSearchApiRequest OriginalRequest { get; set; }   
    }
}