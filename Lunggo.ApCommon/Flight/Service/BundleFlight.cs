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
            if (itineraries.Select(itin => itin.Supplier).All(source => source == itineraries[0].Supplier))
            {
                var conditions = new SpecificSearchConditions
                {
                    FlightSegments = itineraries.SelectMany(itin => itin.FlightTrips).SelectMany(trip => trip.FlightSegments).ToList(),
                    CabinClass = MapCabinClass(itineraries.First().FlightTrips.First().FlightSegments.First().CabinClass),
                    AdultCount = itineraries.First().AdultCount,
                    ChildCount = itineraries.First().ChildCount,
                    InfantCount = itineraries.First().InfantCount
                };
                var result = SpecificSearchFlightInternal(conditions);
                output = result.IsSuccess ? result.FlightItineraries.First() : null;
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
