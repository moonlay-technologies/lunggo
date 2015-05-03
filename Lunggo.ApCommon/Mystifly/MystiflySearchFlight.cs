using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using Lunggo.ApCommon.Dictionary;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Interface;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Hotel.Object;
using Lunggo.ApCommon.Model;
using Lunggo.ApCommon.Mystifly.OnePointService.Flight;
using Lunggo.Flight.Dictionary;
using CabinType = Lunggo.ApCommon.Mystifly.OnePointService.Flight.CabinType;
using PassengerType = Lunggo.ApCommon.Mystifly.OnePointService.Flight.PassengerType;

namespace Lunggo.ApCommon.Mystifly
{
    internal partial class MystiflyWrapper
    {
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
                Target = MystiflyClientHandler.Target,
                ExtensionData = null
            };

            var result = new SearchFlightResult();
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
                            if (error.Code == "ERSER002")
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
                            MapError(response, result);
                        }
                    }
                    result.IsSuccess = false;
                }
            }
            return result;
        }

        private static SearchFlightResult MapResult(AirLowFareSearchRS response, SearchFlightConditions conditions)
        {
            return response.PricedItineraries.Any()
                ? new SearchFlightResult {FlightItineraries = MapFlightFareItineraries(response, conditions)}
                : new SearchFlightResult {FlightItineraries = new List<FlightItineraryFare>()};
        }

        private static OriginDestinationInformation[] MapOriginDestinationInformations(SearchFlightConditions conditions)
        {
            return conditions.TripInfos.Select(info => new OriginDestinationInformation()
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
            var oriDestInfos = conditions.TripInfos;
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

        private static List<FlightItineraryFare> MapFlightFareItineraries(AirLowFareSearchRS response, SearchFlightConditions conditions)
        {
            var result = response.PricedItineraries.Select(itin => MapFlightFareItinerary(itin, conditions)).ToList();
            return result;
        }

        private static FlightItineraryFare MapFlightFareItinerary(PricedItinerary pricedItinerary, ConditionsBase conditions)
        {
            var flightFareItinerary = new FlightItineraryFare();
            flightFareItinerary.FareId = pricedItinerary.AirItineraryPricingInfo.FareSourceCode;
            MapPtcFareBreakdowns(pricedItinerary, flightFareItinerary);
            flightFareItinerary.PscFare =
                decimal.Parse(pricedItinerary.AirItineraryPricingInfo.ItinTotalFare.TotalTax.Amount);
            flightFareItinerary.TotalFare =
                decimal.Parse(pricedItinerary.AirItineraryPricingInfo.ItinTotalFare.TotalFare.Amount);
            flightFareItinerary.TripType = MapTripType(pricedItinerary.DirectionInd.ToString());
            if (pricedItinerary.RequiredFieldsToBook != null)
                MapRequiredFields(pricedItinerary, flightFareItinerary);
            flightFareItinerary.FlightTrips = MapFlightFareTrips(pricedItinerary, conditions);
            flightFareItinerary.Supplier = FlightSupplier.Mystifly;
            flightFareItinerary.CanHold = pricedItinerary.AirItineraryPricingInfo.FareType != FareType.WebFare;
            MapPassengerCount(pricedItinerary, flightFareItinerary);
            return flightFareItinerary;
        }

        private static TripType MapTripType(string type)
        {
            switch (type)
            {
                case "OneWay":
                    return TripType.OneWay;
                case "Return":
                    return TripType.Return;
                case "OpenJaw":
                    return TripType.OpenJaw;
                case "Circle":
                    return TripType.Circle;
                case "Other":
                    return TripType.Other;
                default:
                    return TripType.OpenJaw;
            }
        }

        private static void MapPassengerCount(PricedItinerary pricedItinerary, FlightItineraryFare flightItineraryFare)
        {
            foreach (var item in pricedItinerary.AirItineraryPricingInfo.PTC_FareBreakdowns)
            {
                switch (item.PassengerTypeQuantity.Code)
                {
                    case PassengerType.ADT:
                        flightItineraryFare.AdultCount = item.PassengerTypeQuantity.Quantity;
                        break;
                    case PassengerType.CHD:
                        flightItineraryFare.ChildCount = item.PassengerTypeQuantity.Quantity;
                        break;
                    case PassengerType.INF:
                        flightItineraryFare.InfantCount= item.PassengerTypeQuantity.Quantity;
                        break;
                }
            }
        }

        private static void MapPtcFareBreakdowns(PricedItinerary pricedItinerary, FlightItineraryFare flightItineraryFare)
        {
            var ptcFareBreakdowns = pricedItinerary.AirItineraryPricingInfo.PTC_FareBreakdowns;
            foreach (var ptcFareBreakdown in ptcFareBreakdowns)
            {
                switch (ptcFareBreakdown.PassengerTypeQuantity.Code)
                {
                    case PassengerType.ADT:
                        flightItineraryFare.AdultTotalFare =
                            decimal.Parse(ptcFareBreakdown.PassengerFare.TotalFare.Amount);
                        break;
                    case PassengerType.CHD:
                        flightItineraryFare.ChildTotalFare =
                            decimal.Parse(ptcFareBreakdown.PassengerFare.TotalFare.Amount);
                        break;
                    case PassengerType.INF:
                        flightItineraryFare.InfantTotalFare =
                            decimal.Parse(ptcFareBreakdown.PassengerFare.TotalFare.Amount);
                        break;
                }
            }
        }

        private static void MapRequiredFields(PricedItinerary pricedItinerary, FlightItineraryFare flightItineraryFare)
        {
            foreach (var field in pricedItinerary.RequiredFieldsToBook)
            {
                switch (field)
                {
                    case "Passport":
                        flightItineraryFare.RequirePassport = true;
                        break;
                    case "DOB":
                        flightItineraryFare.RequireBirthDate = true;
                        break;
                    case "SameCheck-InForAllPassengers":
                        flightItineraryFare.RequireSameCheckIn = true;
                        break;
                }
            }
        }

        private static List<FlightTripFare> MapFlightFareTrips(PricedItinerary pricedItinerary, ConditionsBase conditions)
        {
            var flightTrips = new List<FlightTripFare>();
            var segments = pricedItinerary.OriginDestinationOptions.SelectMany(opt => opt.FlightSegments).ToArray();
            var totalTransitDuration = new TimeSpan();
            var i = 0;
            foreach (var tripInfo in conditions.TripInfos)
            {
                var fareTrip = new FlightTripFare
                {
                    OriginAirport = tripInfo.OriginAirport,
                    DestinationAirport = tripInfo.DestinationAirport,
                    DepartureDate = tripInfo.DepartureDate,
                    FlightSegments = new List<FlightSegmentFare>()
                };
                do
                {
                    fareTrip.FlightSegments.Add(MapFlightFareSegment(segments[i]));
                    if (i > 0)
                        totalTransitDuration = totalTransitDuration.Add(segments[i].DepartureDateTime - segments[i - 0].ArrivalDateTime);
                    i++;
                } while (i < segments.Count() && segments[i - 1].ArrivalAirportLocationCode != tripInfo.DestinationAirport);
                flightTrips.Add(fareTrip);
            }
            return flightTrips;
        }

        private static FlightSegmentFare MapFlightFareSegment(FlightSegment flightSegment)
        {
            List<FlightStop> stops = null;
            if (flightSegment.StopQuantity > 0)
            {
                stops = new List<FlightStop>
                    {
                        new FlightStop
                        {
                            Airport = flightSegment.StopQuantityInfo.LocationCode,
                            ArrivalTime = flightSegment.StopQuantityInfo.ArrivalDateTime,
                            DepartureTime = flightSegment.StopQuantityInfo.DepartureDateTime,
                            Duration = TimeSpan.FromMinutes(flightSegment.StopQuantityInfo.Duration)
                        }
                    };
            }
            var segment = new FlightSegmentFare
            {
                DepartureAirport = flightSegment.DepartureAirportLocationCode,
                ArrivalAirport = flightSegment.ArrivalAirportLocationCode,
                DepartureTime = flightSegment.DepartureDateTime,
                ArrivalTime = flightSegment.ArrivalDateTime,
                Duration = TimeSpan.FromMinutes(flightSegment.JourneyDuration),
                AirlineCode = flightSegment.MarketingAirlineCode,
                FlightNumber = flightSegment.FlightNumber,
                OperatingAirlineCode = flightSegment.OperatingAirline.Code,
                AircraftCode = flightSegment.OperatingAirline.Equipment,
                CabinClass = flightSegment.CabinClassCode,
                Rbd = flightSegment.ResBookDesigCode,
                RemainingSeats = flightSegment.SeatsRemaining.Number,
                StopQuantity = flightSegment.StopQuantity,
                FlightStops = stops
            };
            return segment;
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
                    case "ERSER002":
                    case "ERIFS002":
                        if (result.ErrorMessages == null)
                            result.ErrorMessages = new List<string>();
                        result.ErrorMessages.Add("Invalid account information!");
                        goto case "TechnicalError";
                    case "ERSER027":
                        if (result.ErrorMessages == null)
                            result.ErrorMessages = new List<string>();
                        result.ErrorMessages.Add("Daily maximum search limit reached!");
                        goto case "TechnicalError";
                    case "ERGEN002":
                    case "ERGEN018":
                        if (result.ErrorMessages == null)
                            result.ErrorMessages = new List<string>();
                        result.ErrorMessages.Add("Unexpected error on the other end!");
                        goto case "TechnicalError";
                    case "ERMAI001":
                        if (result.ErrorMessages == null)
                            result.ErrorMessages = new List<string>();
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
