using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.WebAPI.ApiSrc.v1.Flights.Model;
using Microsoft.Win32;

namespace Lunggo.CustomerWeb.Models
{
    public class FlightSearchData
    {
        public TripType Trip { get; set; }
        public CabinClass Cabin { get; set; }
        public int Adult { get; set; }
        public int Child { get; set; }
        public int Infant { get; set; }
        public List<OriDestDate> Info { get; set; }
        public string SearchId { get; set; }
        public int TotalFlightCount { get; set; }
        public List<FlightFareItinerary> FlightList { get; set; }
    }
}