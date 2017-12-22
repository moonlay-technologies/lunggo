using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Model.Logic;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Product.Model;
using Lunggo.ApCommon.Sequence;
using Lunggo.Framework.Context;

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
                var token = QuarantineItinerary(input.SearchId, input.RegisterNumbers[0], 0);
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
                var token = QuarantineItinerary(input.SearchId, matchedCombo.BundledRegister, 0);
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

        private string QuarantineItinerary(string searchId, int registerNumber, int partNumber)
        {
            var itinCacheId = FlightItineraryCacheIdSequence.GetInstance().GetNext().ToString(CultureInfo.InvariantCulture);
            var itin = GetItineraryFromSearchCache(searchId, registerNumber, partNumber);

            // TODO: TESTING E2Pay

            var conditions = DecodeSearchConditions(searchId);

            if (conditions.Trips[0].OriginAirport == "XXX" &&
                conditions.Trips[0].DestinationAirport == "XXX")
            {
                itin = new FlightItinerary
                {
                    AdultCount = conditions.AdultCount,
                    ChildCount = conditions.ChildCount,
                    InfantCount = conditions.InfantCount,
                    CanHold = true,
                    FareType = FareType.Published,
                    RequireBirthDate = false,
                    RequirePassport = false,
                    RequireSameCheckIn = false,
                    RequireNationality = false,
                    RequestedCabinClass = conditions.CabinClass,
                    TripType = TripType.OneWay,
                    Supplier = Supplier.LionAir,
                    Price = new Price(),
                    AdultPricePortion = 1,
                    ChildPricePortion = 0,
                    InfantPricePortion = 0,
                    NetAdultPricePortion = 1,
                    FareId = "TEST",
                    RequestedTripType = TripType.OneWay,
                    Trips = new List<FlightTrip>
                            {
                                new FlightTrip
                                {
                                    OriginAirport = conditions.Trips[0].OriginAirport,
                                    DestinationAirport = conditions.Trips[0].DestinationAirport,
                                    DepartureDate = conditions.Trips[0].DepartureDate.Date,
                                    DestinationCity = GetAirportCity(conditions.Trips[0].DestinationAirport),
                                    OriginCity = GetAirportCity(conditions.Trips[0].OriginAirport),
                                    Segments = new List<FlightSegment>
                                    {
                                        new FlightSegment
                                        {
                                            AirlineCode = "XX",
                                            FlightNumber = "1234",
                                            CabinClass = conditions.CabinClass,
                                            AirlineType = AirlineType.Lcc,
                                            DepartureAirport = "XXX",
                                            DepartureTime = new DateTime(2000,1,1,0,0,0),
                                            ArrivalAirport = "XXX",
                                            ArrivalTime = new DateTime(2000,1,1,0,0,0),
                                            OperatingAirlineCode = "XX",
                                            //StopQuantity = Convert.ToInt32(stopNo),
                                            Duration = new TimeSpan(0),
                                            //AircraftCode = aircraftNo,
                                            DepartureCity = "XXX",
                                            ArrivalCity = "XXX",
                                            AirlineName = "XXX",
                                            OperatingAirlineName = "XXX",
                                            IsMealIncluded = false,
                                            IsPscIncluded = true,
                                            IsBaggageIncluded = false,
                                            BaggageCapacity = "0"

                                        }
                                    }
                                }
                            }
                };
                itin.Price.LocalCurrency = new Currency("IDR");
                itin.Price.SetSupplier(15000, new Currency("IDR"));
                itin.Price.Local = 15000;
                itin.Price.SetMargin(new Margin
                {
                    Constant = 0, Percentage = 0, Currency = new Currency("IDR"), Name = "TEST", Description = "TEST"
                });
            }

            if (itin == null)
                return null;

            var currencies = GetCurrencyStatesFromCache(searchId);
            var localCurrency = currencies[OnlineContext.GetActiveCurrencyCode()];
            itin.Price.CalculateFinalAndLocal(localCurrency);
            RoundFinalAndLocalPrice(itin);

            SaveItineraryToCache(itin, itinCacheId);
            return itinCacheId;
        }

        private List<string> QuarantineItineraries(string searchId, IEnumerable<int> registerNumbers)
        {
            return registerNumbers.Select((reg, idx) => QuarantineItinerary(searchId, reg, idx+1)).ToList();
        }
    }
}
