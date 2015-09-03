using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Mystifly.OnePointService.Flight;
using Lunggo.WebAPI.ApiSrc.v1.Flights.Model;
using Microsoft.Win32;
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