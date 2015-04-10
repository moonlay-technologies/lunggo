using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lunggo.ApCommon.Flight.Model;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Model
{
    public class FlightRevalidateApiResponse
    {
        public bool IsValid { get; set; }
        public bool IsOtherFareAvailable { get; set; }
        public FlightFareItinerary Itinerary { get; set; }
        public FlightRevalidateApiRequest OriginalRequest { get; set; }
    }
}