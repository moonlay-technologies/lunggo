using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lunggo.ApCommon.Flight.Model;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Model
{
    public class FlightFilterApiResponse
    {
        public int TotalFlightCount { get; set; }
        public List<FlightItineraryApi> FlightList { get; set; }
        public FlightFilterApiRequest OriginalRequest { get; set; }
        
    }
}