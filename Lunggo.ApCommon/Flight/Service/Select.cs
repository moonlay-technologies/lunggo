using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Model.Logic;
using Lunggo.ApCommon.Sequence;
using Lunggo.Framework.Context;
using RestSharp.Validation;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        public SelectFlightOutput SelectFlight(SelectFlightInput input)
        {
            string tiketToken;
            if (input.RegisterNumbers == null || input.RegisterNumbers.Count == 0)
                return new SelectFlightOutput
                {
                    IsSuccess = true,
                    Token = null
                };

            if (ParseTripType(input.SearchId) == TripType.OneWay)
            {
                var token = QuarantineItinerary(input.SearchId, input.RegisterNumbers[0], 0, out tiketToken);
                var bundledToken = BundleFlight(new List<string> { token });
                if(!string.IsNullOrEmpty(tiketToken))
                
                //if only supplier was Tiket.Com
                SaveTiketTokenToCache(tiketToken, bundledToken);
                var temp = GetTiketTokenInCache(bundledToken);
                
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
                var tokens = QuarantineItineraries(input.SearchId, input.RegisterNumbers);
                var bundledToken = BundleFlight(tokens);
                return new SelectFlightOutput
                {
                    IsSuccess = true,
                    Token = bundledToken
                };
            }

            if (!input.EnableCombo)
            {
                var tokens = QuarantineItineraries(input.SearchId, input.RegisterNumbers);
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
                var token = QuarantineItinerary(input.SearchId, matchedCombo.BundledRegister, 0, out tiketToken);
                var bundledToken = BundleFlight(new List<string> { token });
                return new SelectFlightOutput
                {
                    IsSuccess = true,
                    Token = bundledToken
                };
            }
            else
            {
                var tokens = QuarantineItineraries(input.SearchId, input.RegisterNumbers);
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
            if (tokens == null || tokens.Count == 0 || tokens.Contains(null))
                return null;

            var itins = tokens.Select(GetItineraryFromCache).ToList();

            if (itins.Count == 0 || itins.Contains(null))
                return null;

            var newToken = SaveItinerariesToCache(itins);
            return newToken;
        }

        private string QuarantineItinerary(string searchId, int registerNumber, int partNumber, out string tiketToken)
        {
            tiketToken = null;
            var itinCacheId = FlightItineraryCacheIdSequence.GetInstance().GetNext().ToString(CultureInfo.InvariantCulture);
            var itin = GetItineraryFromSearchCache(searchId, registerNumber, partNumber);

            if (itin == null)
                return null;

            if (itin.Supplier == Supplier.Tiket)
            {
                var flightData = TiketWrapper.SelectFlight(itin.FareId, itin.Trips[0].DepartureDate);
                if (flightData != null && flightData.Required != null)
                {
                    var requireData = flightData.Required;
                    if (requireData.Passportnationalitya1 != null && requireData.Passportnationalitya1.Mandatory == 1)
                    {
                        itin.RequirePassport = true;
                        //itin.RequireNationality = true;
                    }
                    itin.RequireNationality = true;
                    if (requireData.Birthdatea1 != null && requireData.Birthdatea1.Mandatory == 1)
                    {
                        itin.RequireBirthDate = true;
                    }

                    if (requireData.DeptCheckinBaggage != null)
                    {
                        if (requireData.DeptCheckinBaggage.Resource != null &&
                            requireData.DeptCheckinBaggage.Resource.Count != 0)
                        {
                            var capacityData =
                                requireData.DeptCheckinBaggage.Resource.FirstOrDefault(x => x.Name.Contains("(+ IDR 0,00)"));
                            if (capacityData != null)
                            {
                                itin.Trips[0].Segments[0].BaggageCapacity = capacityData.Id;
                            }

                        }
                    }
                    
                    tiketToken = flightData.Token;
                }
            }

            var currencies = GetCurrencyStatesFromCache(searchId);
            var localCurrency = currencies[OnlineContext.GetActiveCurrencyCode()];
            itin.Price.CalculateFinalAndLocal(localCurrency);
            RoundFinalAndLocalPrice(itin);
            
            SaveItineraryToCache(itin, itinCacheId);
            return itinCacheId;
        }

        private List<string> QuarantineItineraries(string searchId, IEnumerable<int> registerNumbers)
        {
            string tiketToken;
            return registerNumbers.Select((reg, idx) => QuarantineItinerary(searchId, reg, idx+1, out tiketToken)).ToList();
        }
    }
}
