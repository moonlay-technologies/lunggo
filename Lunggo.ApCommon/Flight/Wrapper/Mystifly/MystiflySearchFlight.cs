using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Dictionary;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Mystifly.OnePointService.Flight;
using FareType = Lunggo.ApCommon.Mystifly.OnePointService.Flight.FareType;
using FlightSegment = Lunggo.ApCommon.Flight.Model.FlightSegment;
using PassengerType = Lunggo.ApCommon.Mystifly.OnePointService.Flight.PassengerType;

namespace Lunggo.ApCommon.Flight.Wrapper.Mystifly
{
    internal partial class MystiflyWrapper
    {
        private static readonly string[] AirlinesExclude = {"SJ", "IN", "AK", "QZ", "D7", "XT", "QG", "JT", "IW", "ID"};

        internal override SearchFlightResult SearchFlight(SearchFlightConditions conditions)
        {
            var request = new AirLowFareSearchRQ
            {
                OriginDestinationInformations = MapOriginDestinationInformations(conditions),
                IsRefundable = false,
                IsResidentFare = false,
                NearByAirports = false,
                PassengerTypeQuantities = MapPassengerTypes(conditions),
                PricingSourceType = PricingSourceType.All,
                TravelPreferences = MapTravelPreferences(conditions),
                RequestOptions = RequestOptions.TwoHundred,
                SessionId = Client.SessionId,
                Target = Client.Target,
                ExtensionData = null
            };

            var result = new SearchFlightResult { Itineraries = new List<FlightItinerary>() };
            var retry = 0;
            var done = false;
            while (!done)
            {
                var response = Client.AirLowFareSearch(request);
                done = true;
                if ((response.Success && !response.Errors.Any()) ||
                    (response.Errors.Count() == 1 && response.Errors.Single().Code == "ERSER021"))
                {
                    result = MapResult(response, conditions);   
                    result.IsSuccess = true;
                    result.Errors = null;
                }
                else
                {
                    if (response.Errors.Any())
                    {
                        result.Errors = new List<FlightError>();
                        foreach (var error in response.Errors)
                        {
                            if (error.Code == "ERSER001" || error.Code == "ERSER002")
                            {
                                Client.CreateSession();
                                request.SessionId = Client.SessionId;
                                retry++;
                                if (retry <= 3)
                                {
                                    done = false;
                                    break;
                                }
                            }
                        }
                        if (done)
                            MapError(response, result);
                    }
                    result.IsSuccess = false;
                }
            }
            return result;
        }

        private static SearchFlightResult MapResult(AirLowFareSearchRS response, SearchFlightConditions conditions)
        {
            return response.PricedItineraries.Any()
                ? new SearchFlightResult {Itineraries = MapFlightItineraries(response, conditions).Where(itin => itin.CanHold).ToList()}
                : new SearchFlightResult {Itineraries = new List<FlightItinerary>()};
        }

        private static OriginDestinationInformation[] MapOriginDestinationInformations(SearchFlightConditions conditions)
        {
            return conditions.Trips.Select(info => new OriginDestinationInformation()
            {
                DepartureDateTime = DateTime.SpecifyKind(info.DepartureDate,DateTimeKind.Utc),
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
                VendorExcludeCodes = conditions.AirlineExcludes != null ? conditions.AirlineExcludes.ToArray() : AirlinesExclude,
                VendorPreferenceCodes = conditions.AirlinePreferences != null ? conditions.AirlinePreferences.ToArray() : null,
                ExtensionData = null
            };
        }

        private static AirTripType SetAirTripType(SearchFlightConditions conditions)
        {
            AirTripType airTripType;
            var oriDestInfos = conditions.Trips;
            switch (oriDestInfos.Count)
            {
                case 0:
                    airTripType = AirTripType.Other;
                    break;
                case 1:
                    airTripType = AirTripType.OneWay;
                    break;
                case 2:
                    airTripType = (oriDestInfos.First().DestinationAirport == oriDestInfos.Last().OriginAirport &&
                            oriDestInfos.First().OriginAirport == oriDestInfos.Last().DestinationAirport)
                        ? AirTripType.Return
                        : AirTripType.OpenJaw;;
                    break;
                default:
                    var circling = true;
                    for (var i = 1; i < oriDestInfos.Count; i++)
                        if (oriDestInfos[i].OriginAirport != oriDestInfos[i - 1].DestinationAirport)
                            circling = false;
                    if (oriDestInfos.First().OriginAirport != oriDestInfos.Last().DestinationAirport)
                        circling = false;
                    airTripType = circling ? AirTripType.Circle : AirTripType.OpenJaw;
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

        private static List<FlightItinerary> MapFlightItineraries(AirLowFareSearchRS response, SearchFlightConditions conditions)
        {
            return response.PricedItineraries.Select(itin => MapFlightItinerary(itin, conditions)).Where(itin => itin != null).ToList();
        }

        private static FlightItinerary MapFlightItinerary(PricedItinerary pricedItinerary, ConditionsBase conditions)
        {

            if (ItineraryValid(pricedItinerary))
            {
                var flightItinerary = new FlightItinerary();
                flightItinerary.Trips = MapFlightTrips(pricedItinerary, conditions);
                if (flightItinerary.Trips == null)
                    return null;
                flightItinerary.TripType = MapTripType(pricedItinerary.DirectionInd.ToString());
                flightItinerary.SupplierCurrency = "USD";
                flightItinerary.SupplierPrice =
                    decimal.Parse(pricedItinerary.AirItineraryPricingInfo.ItinTotalFare.TotalFare.Amount);
                MapRequiredFields(pricedItinerary, flightItinerary);
                flightItinerary.Supplier = Supplier.Mystifly;
                flightItinerary.FareType = MapFareType(pricedItinerary.AirItineraryPricingInfo.FareType);
                flightItinerary.CanHold = pricedItinerary.AirItineraryPricingInfo.FareType != FareType.WebFare;
                MapPassengerCount(pricedItinerary, flightItinerary);
                flightItinerary.FareId = pricedItinerary.AirItineraryPricingInfo.FareSourceCode;
                return flightItinerary;
            }
            else
            {
                return null;
            }
        }

        private static bool ItineraryValid(PricedItinerary pricedItinerary)
        {
            foreach (var segment in pricedItinerary.OriginDestinationOptions.SelectMany(opt => opt.FlightSegments))
            {
                var dict = DictionaryService.GetInstance();
                if (!dict.IsAirportCodeExists(segment.DepartureAirportLocationCode) ||
                    !dict.IsAirportCodeExists(segment.ArrivalAirportLocationCode))
                {
                    return false;
                }
            }
            foreach (var segment in pricedItinerary.OriginDestinationOptions.SelectMany(opt => opt.FlightSegments))
            {
                var dict = DictionaryService.GetInstance();
                if (!dict.IsAirlineCodeExists(segment.MarketingAirlineCode) ||
                    !dict.IsAirlineCodeExists(segment.OperatingAirline.Code))
                {
                    return false;
                }
            }
            return true;
        }

        private static Flight.Constant.FareType MapFareType(FareType fareType)
        {
            switch (fareType)
            {
                case FareType.Public:
                    return Flight.Constant.FareType.Published;
                case FareType.WebFare:
                    return Flight.Constant.FareType.Lcc;
                case FareType.Private:
                    return Flight.Constant.FareType.Consolidated;
                default:
                    return Flight.Constant.FareType.Undefined;
            }
        }

        private static Flight.Constant.CabinClass MapCabinClass(string cabinClass)
        {
            switch (cabinClass)
            {
                case "Y":
                    return Flight.Constant.CabinClass.Economy;
                case "C":
                    return Flight.Constant.CabinClass.Business;
                case "F":
                    return Flight.Constant.CabinClass.First;
                default:
                    return Flight.Constant.CabinClass.Undefined;
            }
        }

        private static TripType MapTripType(string type)
        {
            switch (type.ToLower())
            {
                case "oneway":
                    return TripType.OneWay;
                case "return":
                    return TripType.Return;
                case "openjaw":
                    return TripType.OpenJaw;
                case "circle":
                    return TripType.Circle;
                case "other":
                    return TripType.Other;
                default:
                    return TripType.OpenJaw;
            }
        }

        private static void MapPassengerCount(PricedItinerary pricedItinerary, FlightItinerary flightItinerary)
        {
            foreach (var item in pricedItinerary.AirItineraryPricingInfo.PTC_FareBreakdowns)
            {
                switch (item.PassengerTypeQuantity.Code)
                {
                    case PassengerType.ADT:
                        flightItinerary.AdultCount = item.PassengerTypeQuantity.Quantity;
                        break;
                    case PassengerType.CHD:
                        flightItinerary.ChildCount = item.PassengerTypeQuantity.Quantity;
                        break;
                    case PassengerType.INF:
                        flightItinerary.InfantCount= item.PassengerTypeQuantity.Quantity;
                        break;
                }
            }
        }
        private static void MapRequiredFields(PricedItinerary pricedItinerary, FlightItinerary flightItinerary)
        {
            if (pricedItinerary.RequiredFieldsToBook != null)
            {
                foreach (var field in pricedItinerary.RequiredFieldsToBook)
                {
                    switch (field.ToLower())
                    {
                        case "passport":
                            flightItinerary.RequirePassport = true;
                            break;
                        case "dob":
                            flightItinerary.RequireBirthDate = true;
                            break;
                        case "samecheck-inforallpassengers":
                            flightItinerary.RequireSameCheckIn = true;
                            break;
                    }
                }
            }
            var dict = DictionaryService.GetInstance();
            var segments = flightItinerary.Trips.SelectMany(trip => trip.Segments).ToList();
            var segmentDepartureAirports = segments.Select(s => s.DepartureAirport);
            var segmentArrivalAirports = segments.Select(s => s.ArrivalAirport);
            var segmentAirports = segmentDepartureAirports.Concat(segmentArrivalAirports);
            var segmentCountries = segmentAirports.Select(dict.GetAirportCountryCode).Distinct();
            if (segmentCountries.Count() > 1)
                flightItinerary.RequirePassport = true;
            else if (flightItinerary.RequirePassport)
            {
                flightItinerary.RequirePassport = false;
                flightItinerary.RequireNationality = true;
            }
        }

        private static List<FlightTrip> MapFlightTrips(PricedItinerary pricedItinerary, ConditionsBase conditions)
        {
            var dict = DictionaryService.GetInstance();
            var flightTrips = new List<FlightTrip>();
            var segments = pricedItinerary.OriginDestinationOptions.SelectMany(opt => opt.FlightSegments).ToArray();
            var i = 0;
            foreach (var tripInfo in conditions.Trips)
            {
                try
                {
                    if (segments[i].DepartureAirportLocationCode != tripInfo.OriginAirport &&
                            dict.GetAirportCityCode(segments[i].DepartureAirportLocationCode) !=
                                 tripInfo.OriginAirport)
                    {
                        return null;
                    }
                    var fareTrip = new FlightTrip
                    {
                        OriginAirport = tripInfo.OriginAirport,
                        DestinationAirport = tripInfo.DestinationAirport,
                        DepartureDate = DateTime.SpecifyKind(tripInfo.DepartureDate,DateTimeKind.Utc),
                        Segments = new List<FlightSegment>()
                    };
                    do
                    {
                        fareTrip.Segments.Add(MapFlightSegment(segments[i]));
                        i++;
                    } while (segments[i - 1].ArrivalAirportLocationCode != tripInfo.DestinationAirport &&
                             dict.GetAirportCityCode(segments[i - 1].ArrivalAirportLocationCode) !=
                                tripInfo.DestinationAirport);
                    flightTrips.Add(fareTrip);
                }
                catch (IndexOutOfRangeException)
                {
                    return null;
                }
            }
            return flightTrips;
        }

        private static FlightSegment MapFlightSegment(ApCommon.Mystifly.OnePointService.Flight.FlightSegment flightSegment)
        {
            List<FlightStop> stops = null;
            if (flightSegment.StopQuantity > 0)
            {
                stops = new List<FlightStop>
                    {
                        new FlightStop
                        {
                            Airport = flightSegment.StopQuantityInfo.LocationCode,
                            ArrivalTime = DateTime.SpecifyKind(flightSegment.StopQuantityInfo.ArrivalDateTime,DateTimeKind.Utc),
                            DepartureTime = DateTime.SpecifyKind(flightSegment.StopQuantityInfo.DepartureDateTime,DateTimeKind.Utc),
                            Duration = TimeSpan.FromMinutes(flightSegment.StopQuantityInfo.Duration)
                        }
                    };
            }
            var segment = new FlightSegment
            {
                DepartureAirport = flightSegment.DepartureAirportLocationCode,
                ArrivalAirport = flightSegment.ArrivalAirportLocationCode,
                DepartureTime = DateTime.SpecifyKind(flightSegment.DepartureDateTime,DateTimeKind.Utc),
                ArrivalTime = DateTime.SpecifyKind(flightSegment.ArrivalDateTime,DateTimeKind.Utc),
                Duration = TimeSpan.FromMinutes(flightSegment.JourneyDuration),
                AirlineCode = flightSegment.MarketingAirlineCode,
                FlightNumber = flightSegment.FlightNumber,
                OperatingAirlineCode = flightSegment.OperatingAirline.Code,
                AircraftCode = flightSegment.OperatingAirline.Equipment,
                Rbd = flightSegment.ResBookDesigCode,
                CabinClass = MapCabinClass(flightSegment.CabinClassCode),
                Meal = !string.IsNullOrEmpty(flightSegment.MealCode),
                RemainingSeats = flightSegment.SeatsRemaining.Number,
                StopQuantity = flightSegment.StopQuantity,
                Stops = stops
            };
            return segment;
        }

        private static void MapError(AirLowFareSearchRS response, ResultBase result)
        {
            foreach (var error in response.Errors)
            {
                switch (error.Code)
                {
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
                    case "ERSER001":
                    case "ERSER002":
                    case "ERIFS001":
                    case "ERIFS002":
                        if (result.ErrorMessages == null)
                            result.ErrorMessages = new List<string>();
                        if (!result.ErrorMessages.Contains("Invalid account information!"))
                            result.ErrorMessages.Add("Invalid account information!");
                        goto case "TechnicalError";
                    case "ERSER027":
                        if (result.ErrorMessages == null)
                            result.ErrorMessages = new List<string>();
                        if (!result.ErrorMessages.Contains("Daily maximum search limit reached!"))
                            result.ErrorMessages.Add("Daily maximum search limit reached!");
                        goto case "TechnicalError";
                    case "ERGEN002":
                    case "ERGEN018":
                        if (result.ErrorMessages == null)
                            result.ErrorMessages = new List<string>();
                        if (!result.ErrorMessages.Contains("Unexpected error on the other end!"))
                            result.ErrorMessages.Add("Unexpected error on the other end!");
                        goto case "TechnicalError";
                    case "ERMAI001":
                        if (result.ErrorMessages == null)
                            result.ErrorMessages = new List<string>();
                        if (!result.ErrorMessages.Contains("Mystifly is under maintenance!"))
                            result.ErrorMessages.Add("Mystifly is under maintenance!");
                        goto case "TechnicalError";

                    case "InvalidInputData":
                        if (!result.Errors.Contains(FlightError.InvalidInputData))
                            result.Errors.Add(FlightError.InvalidInputData);
                        break;
                    case "TechnicalError":
                        if (!result.Errors.Contains(FlightError.TechnicalError))
                            result.Errors.Add(FlightError.TechnicalError);
                        break;
                }
            }
        }
    }
}
