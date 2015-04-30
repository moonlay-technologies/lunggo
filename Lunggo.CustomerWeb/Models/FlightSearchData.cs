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
            public List<FlightTripInfo> TripInfos { get; set; }
            public int AdultCount { get; set; }
            public int ChildCount { get; set; }
            public int InfantCount { get; set; }
            public TripType TripType { get; set; }
            public CabinClass CabinClass { get; set; }
        }

        public static TripType ParseTripType(List<FlightTripInfo> tripInfos)
        {
            switch (tripInfos.Count)
            {
                case 0:
                    return TripType.Undefined;
                case 1:
                    return TripType.OneWay;
                case 2:
                    return (tripInfos.First().DestinationAirport == tripInfos.Last().OriginAirport &&
                            tripInfos.First().OriginAirport == tripInfos.Last().DestinationAirport)
                        ? TripType.Return
                        : TripType.OpenJaw;
                default:
                    var circling = true;
                    for (var i = 1; i < tripInfos.Count; i++)
                        if (tripInfos[i].OriginAirport != tripInfos[i - 1].DestinationAirport)
                            circling = false;
                    if (tripInfos.First().OriginAirport != tripInfos.Last().DestinationAirport)
                        circling = false;
                    return circling ? TripType.Circle : TripType.OpenJaw;
            }
        }

        public static CabinClass ParseCabinClass(string code)
        {
            switch (code.ToUpper())
            {
                case "Y":
                    return CabinClass.Economy;
                case "C":
                    return CabinClass.Business;
                case "F":
                    return CabinClass.First;
                default:
                    return CabinClass.Undefined;
            }
        }
    }
}