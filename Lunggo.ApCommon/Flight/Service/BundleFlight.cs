using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Model;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        public string BundleFlight(List<string> tokens)
        {
            var itins = tokens.Select(GetItineraryFromCache).ToList();
            var itinBundle = BundleItineraries(itins);
            var newToken = SaveItinerarySetAndBundleToCache(itins, itinBundle);
            return newToken;
        }

        internal FlightItinerary BundleItineraries(List<FlightItinerary> itins)
        {
            var itin = new FlightItinerary
            {
                AdultCount = itins[0].AdultCount,
                ChildCount = itins[0].ChildCount,
                InfantCount = itins[0].InfantCount,
                CabinClass = itins[0].CabinClass,
                CanHold = itins.TrueForAll(i => i.CanHold),
                FinalIdrPrice = itins.Sum(i => i.FinalIdrPrice),
                RequireBirthDate = itins.Any(i => i.RequireBirthDate),
                RequirePassport = itins.Any(i => i.RequirePassport),
                RequireSameCheckIn = itins.Any(i => i.RequireSameCheckIn),
                FlightTrips =
                    itins.SelectMany(i => i.FlightTrips).OrderBy(t => t.FlightSegments[0].DepartureTime).ToList()
            };
            return itin;
        }
    }
}
