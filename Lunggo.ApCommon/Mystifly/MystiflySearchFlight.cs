using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Lunggo.ApCommon.Flight.Interface;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Hotel.Object;
using Lunggo.ApCommon.Model;
using Lunggo.ApCommon.Mystifly.OnePointService.Flight;

namespace Lunggo.ApCommon.Mystifly
{
    public partial class MystiflyWrapper : ISearchFlight
    {
        public SearchFlightResult SearchFlight(SearchFlightConditions conditions)
        {
            var originDestinationInformations = MapOriginDestinationInformations(conditions);
            var passengerTypes = MapPassengerTypes(conditions);
            var travelPreferences = MapTravelPreferences(conditions);

            using (var client = new MystiflyClientHandler())
            {
                var request = new AirLowFareSearchRQ
                {
                    OriginDestinationInformations = originDestinationInformations.ToArray(),
                    IsRefundable = true,
                    IsResidentFare = false,
                    NearByAirports = false,
                    PassengerTypeQuantities = passengerTypes.ToArray(),
                    PricingSourceType = PricingSourceType.Public,
                    TravelPreferences = travelPreferences,
                    RequestOptions = RequestOptions.TwoHundred,
                    SessionId = client.SessionId,
                    Target = client.Target,
                    ExtensionData = null
                };

                var result = new SearchFlightResult();
                var retry = 0;
                var done = false;
                while (retry < 3 && !done)
                {
                    var response = client.AirLowFareSearch(request);
                    done = true;
                    if (!response.Errors.Any())
                    {
                        result = MapResult(response);
                        result.Success = true;
                    }
                    else
                    {
                        foreach (var error in response.Errors)
                        {
                            if (error.Code == "ERSER002")
                            {
                                result.Errors.Clear();
                                client.CreateSession();
                                request.SessionId = client.SessionId;
                                retry++;
                                done = false;
                                break;
                            }
                            result.Errors.Add(MapError(error));
                            result.Success = false;
                        }
                    }
                
                }
                return result;
            }
        }

        private static SearchFlightResult MapResult(AirLowFareSearchRS response)
        {
            return new SearchFlightResult{ FlightItineraries = MapFlightFareItineraries(response) };
        }

        private static List<OriginDestinationInformation> MapOriginDestinationInformations(SearchFlightConditions conditions)
        {
            return conditions.OriDestInfos.Select(info => new OriginDestinationInformation()
            {
                DepartureDateTime = info.DepartureDate,
                DestinationLocationCode = info.DestinationAirport,
                OriginLocationCode = info.OriginAirport,
                ArrivalWindow = null,
                DepartureWindow = null,
                ExtensionData = null
            }).ToList();
        }

        private static List<PassengerTypeQuantity> MapPassengerTypes(SearchFlightConditions conditions)
        {
            var passengerTypeQuantities = new List<PassengerTypeQuantity>
            {
                new PassengerTypeQuantity()
                {
                    Code = PassengerType.ADT,
                    Quantity = conditions.AdultCount
                }
            };
            if (conditions.ChildCount > 0)
                passengerTypeQuantities.Add(
                    new PassengerTypeQuantity()
                    {
                        Code = PassengerType.CHD,
                        Quantity = conditions.ChildCount
                    });
            if (conditions.InfantCount > 0)
                passengerTypeQuantities.Add(
                    new PassengerTypeQuantity()
                    {
                        Code = PassengerType.INF,
                        Quantity = conditions.InfantCount
                    });
            return passengerTypeQuantities;
        }

        private static TravelPreferences MapTravelPreferences(SearchFlightConditions conditions)
        {
            var airTripType = SetAirTripType(conditions);
            var cabinType = SetCabinType(conditions);
            return new TravelPreferences
            {
                AirTripType = airTripType,
                CabinPreference = cabinType,
                MaxStopsQuantity = MaxStopsQuantity.All,
                VendorExcludeCodes = null,
                VendorPreferenceCodes = null,
                ExtensionData = null
            };
        }

        private static AirTripType SetAirTripType(SearchFlightConditions conditions)
        {
            AirTripType airTripType;
            var oriDestInfos = conditions.OriDestInfos;
            switch (oriDestInfos.Count)
            {
                case 1:
                    airTripType = AirTripType.OneWay;
                    break;
                case 2:
                    airTripType = AirTripType.Return;
                    break;
                default:
                    airTripType = oriDestInfos.First().OriginAirport == oriDestInfos.Last().DestinationAirport
                        ? AirTripType.Circle
                        : AirTripType.OpenJaw;
                    break;
            }
            return airTripType;
        }

        private static CabinType SetCabinType(SearchFlightConditions conditions)
        {
            CabinType cabinType;
            switch (conditions.CabinType)
            {
                case Flight.Constant.CabinType.Economy:
                    cabinType = CabinType.Y;
                    break;
                case Flight.Constant.CabinType.Business:
                    cabinType = CabinType.C;
                    break;
                case Flight.Constant.CabinType.First:
                    cabinType = CabinType.F;
                    break;
                default:
                    cabinType = CabinType.Default;
                    break;
            }
            return cabinType;
        }

        private static List<FlightFareItinerary> MapFlightFareItineraries(AirLowFareSearchRS response)
        {
            var result = response.PricedItineraries.Select(MapFlightFareItinerary).ToList();
            return result;
        }

        private static FlightFareItinerary MapFlightFareItinerary(PricedItinerary pricedItinerary)
        {
            var flightFareItinerary = new FlightFareItinerary();
            flightFareItinerary.FareId = pricedItinerary.AirItineraryPricingInfo.FareSourceCode;
            MapPTCFareBreakdowns(pricedItinerary, flightFareItinerary);
            flightFareItinerary.TotalFare =
                decimal.Parse(pricedItinerary.AirItineraryPricingInfo.ItinTotalFare.TotalFare.Amount);
            flightFareItinerary.Currency = pricedItinerary.AirItineraryPricingInfo.ItinTotalFare.TotalFare.CurrencyCode;
            flightFareItinerary.TripType = pricedItinerary.DirectionInd.ToString();
            if (pricedItinerary.RequiredFieldsToBook != null)
                MapRequiredFields(pricedItinerary, flightFareItinerary);
            flightFareItinerary.FlightTrips = MapFlightTrips(pricedItinerary);
            return flightFareItinerary;
        }

        private static void MapPTCFareBreakdowns(PricedItinerary pricedItinerary, FlightFareItinerary flightFareItinerary)
        {
            var ptcFareBreakdowns = pricedItinerary.AirItineraryPricingInfo.PTC_FareBreakdowns;
            foreach (var ptcFareBreakdown in ptcFareBreakdowns)
            {
                switch (ptcFareBreakdown.PassengerTypeQuantity.Code)
                {
                    case PassengerType.ADT:
                        flightFareItinerary.AdultTotalFare =
                            decimal.Parse(ptcFareBreakdown.PassengerFare.TotalFare.Amount);
                        break;
                    case PassengerType.CHD:
                        flightFareItinerary.ChildTotalFare =
                            decimal.Parse(ptcFareBreakdown.PassengerFare.TotalFare.Amount);
                        break;
                    case PassengerType.INF:
                        flightFareItinerary.InfantTotalFare =
                            decimal.Parse(ptcFareBreakdown.PassengerFare.TotalFare.Amount);
                        break;
                }
            }
        }

        private static void MapRequiredFields(PricedItinerary pricedItinerary, FlightFareItinerary flightFareItinerary)
        {
            foreach (var field in pricedItinerary.RequiredFieldsToBook)
            {
                switch (field)
                {
                    case "Passport":
                        flightFareItinerary.RequirePassport = true;
                        break;
                    case "DOB":
                        flightFareItinerary.RequireBirthDate = true;
                        break;
                    case "SameCheck-InForAllPassengers":
                        flightFareItinerary.RequireSameCheckIn = true;
                        break;
                }
            }
        }

        private static List<FlightFareTrip> MapFlightTrips(PricedItinerary pricedItinerary)
        {
            var flightTrips = new List<FlightFareTrip>();
            foreach (var originDestinationOptions in pricedItinerary.OriginDestinationOptions)
            {
                foreach (var flightSegment in originDestinationOptions.FlightSegments)
                {
                    var flightStops = new List<FlightStop>();
                    if (flightSegment.StopQuantity > 0)
                    {
                        var flightStop = new FlightStop
                        {
                            ArrivalTime = flightSegment.StopQuantityInfo.ArrivalDateTime,
                            DepartureTime = flightSegment.StopQuantityInfo.DepartureDateTime,
                            Duration = flightSegment.StopQuantityInfo.Duration,
                            StopAirport = flightSegment.StopQuantityInfo.LocationCode
                        };
                        flightStops.Add(flightStop);
                    }
                    var flightTrip = new FlightFareTrip
                    {
                        DepartureAirport = flightSegment.DepartureAirportLocationCode,
                        ArrivalAirport = flightSegment.ArrivalAirportLocationCode,
                        DepartureTime = flightSegment.DepartureDateTime,
                        ArrivalTime = flightSegment.ArrivalDateTime,
                        AirlineCode = flightSegment.OperatingAirline.Code,
                        FlightNumber = flightSegment.OperatingAirline.FlightNumber,
                        Duration = flightSegment.JourneyDuration,
                        CabinClass = flightSegment.CabinClassCode,
                        RBD = flightSegment.ResBookDesigCode,
                        RemainingSeats = flightSegment.SeatsRemaining.Number,
                        StopQuantity = flightSegment.StopQuantity,
                        FlightStops = flightStops
                    };
                    flightTrips.Add(flightTrip);
                }
            }
            return flightTrips;
        }
    }
}
