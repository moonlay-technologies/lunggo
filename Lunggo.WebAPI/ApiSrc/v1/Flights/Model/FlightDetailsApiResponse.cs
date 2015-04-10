using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lunggo.ApCommon.Flight.Model;

namespace Lunggo.WebAPI.ApiSrc.v1.Flights.Model
{
    public class FlightDetailsApiResponse
    {
        public string BookingId { get; set; }
        public int FlightSegmentCount { get; set; }
        public FlightItineraryDetails TripDetails { get; set; }
        public List<string> BookingNotes { get; set; }
        public FlightDetailsApiRequest OriginalRequest { get; set; }
    }
}