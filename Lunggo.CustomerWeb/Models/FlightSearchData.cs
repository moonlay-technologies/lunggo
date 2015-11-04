using System.Collections.Generic;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using CabinClass = Lunggo.ApCommon.Flight.Constant.CabinClass;

namespace Lunggo.CustomerWeb.Models
{
    public class FlightSearchData
    {
        public string info { get; set; }

        public class Complete
        {
            public List<FlightTrip> Trips { get; set; }
            public int AdultCount { get; set; }
            public int ChildCount { get; set; }
            public int InfantCount { get; set; }
            public TripType TripType { get; set; }
            public CabinClass CabinClass { get; set; }
        }
    }
}