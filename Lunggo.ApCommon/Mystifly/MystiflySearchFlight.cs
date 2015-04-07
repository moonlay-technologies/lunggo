using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Interface;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Hotel.Object;
using Lunggo.ApCommon.Model;
using Lunggo.ApCommon.Mystifly.OnePointService.Flight;
using CabinType = Lunggo.ApCommon.Mystifly.OnePointService.Flight.CabinType;
using PassengerType = Lunggo.ApCommon.Mystifly.OnePointService.Flight.PassengerType;

namespace Lunggo.ApCommon.Mystifly
{
    internal partial class MystiflyWrapper
    {
        internal override SearchFlightResult SearchFlight(SearchFlightConditions conditions)
        {
            using (var client = new MystiflyClientHandler())
            {
                var request = new AirLowFareSearchRQ
                {
                    OriginDestinationInformations = MapOriginDestinationInformations(conditions),
                    IsRefundable = true,
                    IsResidentFare = false,
                    NearByAirports = false,
                    PassengerTypeQuantities = MapPassengerTypes(conditions),
                    PricingSourceType = PricingSourceType.Public,
                    TravelPreferences = MapTravelPreferences(conditions),
                    RequestOptions = RequestOptions.TwoHundred,
                    SessionId = client.SessionId,
                    Target = MystiflyClientHandler.Target,
                    ExtensionData = null
                };

                var result = new SearchFlightResult();
                var retry = 0;
                var done = false;
                while (!done)
                {
                    var response = client.AirLowFareSearch(request);
                    request.IsRefundable = false;
                    var refundResponse = client.AirLowFareSearch(request);
                    done = true;
                    if (!response.Errors.Any() || !refundResponse.Errors.Any())
                    {
                        result.FlightItineraries = new List<FlightFareItinerary>();
                        if (!response.Errors.Any())
                        {
                            var result1 = MapResult(response);
                            result.FlightItineraries.AddRange(result1.FlightItineraries);
                        }
                        if (!refundResponse.Errors.Any())
                        {
                            var result2 = MapResult(refundResponse);
                            result.FlightItineraries.AddRange(result2.FlightItineraries);
                        }
                        result.IsSuccess = true;
                    }
                    else
                    {
                        var errors = response.Errors.Concat(refundResponse.Errors).Distinct();
                        foreach (var error in errors)
                        {
                            if (error.Code == "ERSER002")
                            {
                                result.Errors.Clear();
                                client.CreateSession();
                                request.SessionId = client.SessionId;
                                retry++;
                                if (retry <= 3)
                                {
                                    done = false;
                                    break;
                                }
                            }
                            MapError(response, result);
                        }
                        result.IsSuccess = false;
                    }
                }
                
                return result;
            }
        }

        private static SearchFlightResult MapResult(AirLowFareSearchRS response)
        {
            return new SearchFlightResult{ FlightItineraries = MapFlightFareItineraries(response) };
        }

        private static OriginDestinationInformation[] MapOriginDestinationInformations(SearchFlightConditions conditions)
        {
            return conditions.OriDestInfos.Select(info => new OriginDestinationInformation()
            {
                DepartureDateTime = info.DepartureDate,
                DestinationLocationCode = info.DestinationAirport,
                OriginLocationCode = info.OriginAirport,
                ArrivalWindow = null,
                DepartureWindow = null,
                ExtensionData = null
            }).ToArray();
        }

        private static PassengerTypeQuantity[] MapPassengerTypes(SearchFlightConditions conditions)
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
            return passengerTypeQuantities.ToArray();
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
            switch (conditions.CabinClass)
            {
                case Flight.Constant.CabinClass.Economy:
                    cabinType = CabinType.Y;
                    break;
                case Flight.Constant.CabinClass.Business:
                    cabinType = CabinType.C;
                    break;
                case Flight.Constant.CabinClass.First:
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
            flightFareItinerary.PSCFare =
                decimal.Parse(pricedItinerary.AirItineraryPricingInfo.ItinTotalFare.TotalTax.Amount);
            flightFareItinerary.TotalFare =
                decimal.Parse(pricedItinerary.AirItineraryPricingInfo.ItinTotalFare.TotalFare.Amount);
            flightFareItinerary.Currency = pricedItinerary.AirItineraryPricingInfo.ItinTotalFare.TotalFare.CurrencyCode;
            flightFareItinerary.TripType = pricedItinerary.DirectionInd.ToString();
            if (pricedItinerary.RequiredFieldsToBook != null)
                MapRequiredFields(pricedItinerary, flightFareItinerary);
            flightFareItinerary.FlightTrips = MapFlightTrips(pricedItinerary);
            flightFareItinerary.Source = FlightSource.Wholesaler;
            MapPassengerCount(pricedItinerary, flightFareItinerary);
            return flightFareItinerary;
        }

        private static void MapPassengerCount(PricedItinerary pricedItinerary, FlightFareItinerary flightFareItinerary)
        {
            foreach (var item in pricedItinerary.AirItineraryPricingInfo.PTC_FareBreakdowns)
            {
                switch (item.PassengerTypeQuantity.Code)
                {
                    case PassengerType.ADT:
                        flightFareItinerary.AdultCount = item.PassengerTypeQuantity.Quantity;
                        break;
                    case PassengerType.CHD:
                        flightFareItinerary.ChildCount = item.PassengerTypeQuantity.Quantity;
                        break;
                    case PassengerType.INF:
                        flightFareItinerary.InfantCount= item.PassengerTypeQuantity.Quantity;
                        break;
                }
            }
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
            foreach (var flightSegment in pricedItinerary.OriginDestinationOptions.SelectMany(options => options.FlightSegments))
            {
                if (flightSegment.StopQuantity > 0)
                {
                    var firstFlightTrip = new FlightFareTrip
                    {
                        DepartureAirport = flightSegment.DepartureAirportLocationCode,
                        ArrivalAirport = flightSegment.StopQuantityInfo.LocationCode,
                        DepartureTime = flightSegment.DepartureDateTime,
                        ArrivalTime = flightSegment.StopQuantityInfo.ArrivalDateTime,
                        AirlineCode = flightSegment.OperatingAirline.Code,
                        FlightNumber = flightSegment.OperatingAirline.FlightNumber,
                        AircraftCode = flightSegment.OperatingAirline.Equipment,
                        Duration = flightSegment.JourneyDuration,
                        CabinClass = flightSegment.CabinClassCode,
                        RBD = flightSegment.ResBookDesigCode,
                        RemainingSeats = flightSegment.SeatsRemaining.Number
                    };
                    var secondFlightTrip = new FlightFareTrip
                    {
                        DepartureAirport = flightSegment.StopQuantityInfo.LocationCode,
                        ArrivalAirport = flightSegment.ArrivalAirportLocationCode,
                        DepartureTime = flightSegment.StopQuantityInfo.DepartureDateTime,
                        ArrivalTime = flightSegment.ArrivalDateTime,
                        AirlineCode = flightSegment.OperatingAirline.Code,
                        FlightNumber = flightSegment.OperatingAirline.FlightNumber,
                        AircraftCode = flightSegment.OperatingAirline.Equipment,
                        Duration = 0,
                        CabinClass = flightSegment.CabinClassCode,
                        RBD = flightSegment.ResBookDesigCode,
                        RemainingSeats = flightSegment.SeatsRemaining.Number
                    };
                    flightTrips.Add(firstFlightTrip);
                    flightTrips.Add(secondFlightTrip);
                }
                else
                {
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
                        RemainingSeats = flightSegment.SeatsRemaining.Number
                    };
                    flightTrips.Add(flightTrip);
                }
            }
            return flightTrips;
        }

        private static void MapError(AirLowFareSearchRS response, ResultBase result)
        {
            foreach (var error in response.Errors)
            {
                switch (error.Code)
                {
                    case "ERSER001":
                    case "ERSER003":
                    case "ERSER004":
                    case "ERSER005":
                    case "ERSER006":
                    case "ERSER007":
                    case "ERSER008":
                    case "ERSER009":
                    case "ERSER010":
                    case "ERSER011":
                    case "ERSER012":
                    case "ERSER013":
                    case "ERSER014":
                    case "ERSER015":
                    case "ERSER016":
                    case "ERSER017":
                    case "ERSER018":
                    case "ERSER019":
                    case "ERSER020":
                    case "ERSER022":
                    case "ERSER023":
                    case "ERSER024":
                    case "ERSER025":
                    case "ERSER026":
                    case "ERIFS001":
                    case "ERIFS003":
                    case "ERIFS004":
                    case "ERIFS005":
                    case "ERIFS006":
                    case "ERIFS007":
                    case "ERIFS008":
                    case "ERIFS009":
                    case "ERIFS010":
                    case "ERIFS011":
                    case "ERIFS015":
                    case "ERIFS016":
                        goto case "InvalidInputData";
                    case "ERSER027":
                        result.ErrorMessages.Add("Daily maximum search limit reached!");
                        goto case "TechnicalError";
                    case "ERGEN002":
                        result.ErrorMessages.Add("Unexpected error on the other end!");
                        goto case "TechnicalError";
                    case "ERGEN018":
                        result.ErrorMessages.Add("Unexpected error on the other end!");
                        goto case "TechnicalError";
                    case "ERMAI001":
                        result.ErrorMessages.Add("Mystifly is under maintenance!");
                        goto case "TechnicalError";
                    case "InvalidInputData":
                        result.Errors.Add(FlightError.InvalidInputData);
                        break;
                    case "TechnicalError":
                        result.Errors.Add(FlightError.TechnicalError);
                        break;
                }
            }
        }
    }
}
