using System.Collections.Generic;
using System.Linq;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        public string SelectFlight(string searchId, List<int> registerNumbers)
        {
            if (ParseTripType(searchId) == TripType.Return)
            {
                var depItin = GetItineraryFromSearchCache(searchId, registerNumbers[0], 1);
                var retItin = GetItineraryFromSearchCache(searchId, registerNumbers[1], 2);
                var depCombo = depItin.ComboSet;
                var retCombo = retItin.ComboSet;
                if (depCombo != null && retCombo != null)
                {
                    var depBundleRegNoIdx = depCombo.PairRegisterNumber.IndexOf(registerNumbers[1]);
                    var retBundleRegNoIdx = retCombo.PairRegisterNumber.IndexOf(registerNumbers[0]);
                    if (depBundleRegNoIdx != -1 && retBundleRegNoIdx != -1)
                    {
                        var comboRegByDep = depCombo.BundledRegisterNumber[depBundleRegNoIdx];
                        var comboRegByRet = retCombo.BundledRegisterNumber[retBundleRegNoIdx];
                        if (comboRegByDep != -1 && comboRegByRet != -1 && comboRegByDep == comboRegByRet)
                        {
                            var token = SaveItineraryFromSearchToCache(searchId, comboRegByDep, 0);
                            var newToken = BundleFlight(new List<string> {token});
                            return newToken;
                        }
                    }
                }
                var depToken = SaveItineraryFromSearchToCache(searchId, registerNumbers[0]);
                var retToken = SaveItineraryFromSearchToCache(searchId, registerNumbers[1]);
                var bundledToken = BundleFlight(new List<string> {depToken, retToken});
                return bundledToken;
            }
            else
            {
                var token = SaveItineraryFromSearchToCache(searchId, registerNumbers[0]);
                var newToken = BundleFlight(new List<string> { token });
                return newToken;
            }
        }

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
                RequestedCabinClass = itins[0].RequestedCabinClass,
                CanHold = itins.TrueForAll(i => i.CanHold),
                FinalIdrPrice = itins.Sum(i => i.FinalIdrPrice),
                RequireBirthDate = itins.Any(i => i.RequireBirthDate),
                RequirePassport = itins.Any(i => i.RequirePassport),
                RequireSameCheckIn = itins.Any(i => i.RequireSameCheckIn),
                RequireNationality = itins.Any(i => i.RequireNationality),
                LocalPrice = itins.Sum(i => i.LocalPrice),
                Trips =
                    itins.SelectMany(i => i.Trips).OrderBy(t => t.Segments[0].DepartureTime).ToList()
            };
            return itin;
        }
    }
}
