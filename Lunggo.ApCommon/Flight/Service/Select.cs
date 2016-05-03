using System.Collections.Generic;
using System.Linq;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Model.Logic;
using Lunggo.ApCommon.ProductBase.Model;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        public SelectFlightOutput SelectFlight(SelectFlightInput input)
        {
            if (input.RegisterNumbers == null || input.RegisterNumbers.Count == 0)
                return new SelectFlightOutput
                {
                    IsSuccess = true,
                    Token = null
                };

            if (ParseTripType(input.SearchId) == TripType.OneWay)
            {
                var token = SaveItineraryFromSearchToCache(input.SearchId, input.RegisterNumbers[0], 0);
                var bundledToken = BundleFlight(new List<string> { token });
                return new SelectFlightOutput
                {
                    IsSuccess = true,
                    Token = bundledToken
                };
            }

            var supplierIndices = input.RegisterNumbers.Select(reg => reg / SupplierIndexCap).Distinct().ToList();
            var isFromSameSupplier = supplierIndices.Count == 1;

            if (!isFromSameSupplier)
            {
                var tokens = input.RegisterNumbers.Select(reg => SaveItineraryFromSearchToCache(input.SearchId, reg, input.RegisterNumbers.IndexOf(reg) + 1)).ToList();
                var bundledToken = BundleFlight(tokens);
                return new SelectFlightOutput
                {
                    IsSuccess = true,
                    Token = bundledToken
                };
            }

            var combos = GetCombosFromCache(input.SearchId, supplierIndices[0]);
            var matchedCombo = combos.SingleOrDefault(combo =>
            {
                var allMatched = true;
                for (var i = 0; i < combo.Registers.Length; i++)
                {
                    if (combo.Registers[i] != input.RegisterNumbers[i])
                        allMatched = false;
                }
                return allMatched;
            });
            if (matchedCombo != null)
            {
                var token = SaveItineraryFromSearchToCache(input.SearchId, matchedCombo.BundledRegister, 0);
                var bundledToken = BundleFlight(new List<string> { token });
                return new SelectFlightOutput
                {
                    IsSuccess = true,
                    Token = bundledToken
                };
            }
            else
            {
                var tokens = input.RegisterNumbers.Select(reg => SaveItineraryFromSearchToCache(input.SearchId, reg, input.RegisterNumbers.IndexOf(reg) + 1)).ToList();
                var bundledToken = BundleFlight(tokens);
                return new SelectFlightOutput
                {
                    IsSuccess = true,
                    Token = bundledToken
                };
            }
        }

        public string BundleFlight(List<string> tokens)
        {
            if (tokens.Count == 0 || tokens.Contains(null))
                return null;

            var itins = tokens.Select(GetItineraryFromCache).ToList();
            var newToken = SaveItinerariesToCache(itins);
            return newToken;
        }
    }
}
