using System.Collections.Generic;
using System.Linq;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        public FlightFareItinerary BundleFlight(List<FlightFareItinerary> itineraries)
        {
            FlightFareItinerary output;
            if (itineraries.Select(itin => itin.Source).All(source => source == itineraries[0].Source))
            {
                var conditions = new SpecificSearchConditions
                {
                    FlightTrips = itineraries.SelectMany(itin => itin.FlightTrips).ToList(),
                    CabinClass = MapCabinClass(itineraries.First().FlightTrips.First().CabinClass),
                    AdultCount = itineraries.First().AdultCount,
                    ChildCount = itineraries.First().ChildCount,
                    InfantCount = itineraries.First().InfantCount
                };
                var result = SpecificSearchFlightInternal(conditions);
                output = result.IsSuccess ? Enumerable.First<FlightFareItinerary>(result.FlightItineraries) : null;
            }
            else
            {
                output = null;
            }
            return output;
        }

        private static CabinClass MapCabinClass(string cabinClass)
        {
            switch (cabinClass)
            {
                case "Y":
                    return CabinClass.Economy;
                case "D":
                    return CabinClass.Business;
                case "F":
                    return CabinClass.First;
                default:
                    return CabinClass.Economy;
            }
        }
    }
}
