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
            if (ParseTripType(searchId) == TripType.OneWay)
            {
                var token = SaveItineraryFromSearchToCache(searchId, registerNumbers[0], 0);
                var bundledToken = BundleFlight(new List<string> { token });
                return bundledToken;
            }
            else
            {
                var supplierIndices = registerNumbers.Select(reg => reg / SupplierIndexCap).Distinct().ToList();
                var isFromSameSupplier = supplierIndices.Count == 1;

                if (!isFromSameSupplier)
                {
                    var tokens = registerNumbers.Select(reg => SaveItineraryFromSearchToCache(searchId, reg, registerNumbers.IndexOf(reg) + 1)).ToList();
                    var bundledToken = BundleFlight(tokens);
                    return bundledToken;
                }

                var combos = GetCombosFromCache(searchId, supplierIndices[0]);
                var matchedCombo = combos.SingleOrDefault(combo =>
                {
                    var allMatched = true;
                    for (var i = 0; i < combo.Registers.Length; i++)
                    {
                        if (combo.Registers[i] != registerNumbers[i])
                            allMatched = false;
                    }
                    return allMatched;
                });
                if (matchedCombo != null)
                {
                    var token = SaveItineraryFromSearchToCache(searchId, matchedCombo.BundledRegister, 0);
                    var bundledToken = BundleFlight(new List<string> { token });
                    return bundledToken;
                }
                else
                {
                    var tokens = registerNumbers.Select(reg => SaveItineraryFromSearchToCache(searchId, reg, registerNumbers.IndexOf(reg) + 1)).ToList();
                    var bundledToken = BundleFlight(tokens);
                    return bundledToken;
                }
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
