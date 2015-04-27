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
                }
                else
                {
                    if (response.Errors.Any())
                    {
                        result.Errors = new List<FlightError>();
                        result.ErrorMessages = new List<string>();
                        foreach (var error in response.Errors)
                        {
                            if (error.Code == "ERSER002")
                            {
                                result.Errors = null;
                                result.ErrorMessages = null;
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
                : new SearchFlightResult {FlightItineraries = new List<FlightFareItinerary>()};
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

        private static List<FlightFareItinerary> MapFlightFareItineraries(AirLowFareSearchRS response, SearchFlightConditions conditions)
        {
            var result = response.PricedItineraries.Select(itin => MapFlightFareItinerary(itin, conditions)).ToList();
            return result;
        }

        private static FlightFareItinerary MapFlightFareItinerary(PricedItinerary pricedItinerary, ConditionsBase conditions)
        {
            var flightFareItinerary = new FlightFareItinerary();
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
            flightFareItinerary.TotalTransit = CalculateTotalTransit(pricedItinerary);
            flightFareItinerary.Transits = MapTransitDetails(pricedItinerary);
            flightFareItinerary.Airlines = GetAirlineList(pricedItinerary);
            flightFareItinerary.Supplier = FlightSupplier.Mystifly;
            flightFareItinerary.CanHold = pricedItinerary.AirItineraryPricingInfo.FareType != FareType.WebFare;
            MapPassengerCount(pricedItinerary, flightFareItinerary);
            return flightFareItinerary;
        }

        private static List<Airline> GetAirlineList(PricedItinerary pricedItinerary)
        {
            var segments = pricedItinerary.OriginDestinationOptions.SelectMany(opt => opt.FlightSegments).Distinct();
            var airlines = segments.Select(segment => new Airline
            {
                Code = segment.MarketingAirlineCode,
                Name = DictionaryService.GetInstance().GetAirlineName(segment.MarketingAirlineCode)
            });
            return airlines.ToList();
        }

        private static List<Transit> MapTransitDetails(PricedItinerary itin)
        {
            var segments = itin.OriginDestinationOptions.SelectMany(opt => opt.FlightSegments).ToList();
            var result = new List<Transit>();
            for (var i = 0; i < segments.Count; i++)
            {
                if (segments[i].StopQuantity > 0)
                {
                    result.Add(new Transit
                    {
                        IsStop = true,
                        Location = segments[i].StopQuantityInfo.LocationCode,
                        Arrival = segments[i].StopQuantityInfo.ArrivalDateTime,
                        Departure = segments[i].StopQuantityInfo.DepartureDateTime,
                    });
                }
                if (i != 0)
                {
                    result.Add(new Transit
                    {
                        IsStop = false,
                        Location = segments[i].DepartureAirportLocationCode,
                        Arrival = segments[i - 1].ArrivalDateTime,
                        Departure = segments[i].DepartureDateTime
                    });
                }
            }
            return result;
        }

        private static int CalculateTotalTransit(PricedItinerary itin)
        {
            var segments = itin.OriginDestinationOptions.SelectMany(opt => opt.FlightSegments).ToList();
            var transit = segments.Count() - 1;
            var stop = segments.Sum(segment => segment.StopQuantity);
            return transit + stop;
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

        private static void MapPtcFareBreakdowns(PricedItinerary pricedItinerary, FlightFareItinerary flightFareItinerary)
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

        private static List<FlightFareTrip> MapFlightFareTrips(PricedItinerary pricedItinerary, ConditionsBase conditions)
        {
            var flightTrips = new List<FlightFareTrip>();
            var segments = pricedItinerary.OriginDestinationOptions.SelectMany(opt => opt.FlightSegments).ToArray();
            var totalTransitDuration = new TimeSpan();
            var i = 0;
            foreach (var tripInfo in conditions.TripInfos)
            {
                var fareTrip = new FlightFareTrip
                {
                    OriginAirport = tripInfo.OriginAirport,
                    DestinationAirport = tripInfo.DestinationAirport,
                    DepartureDate = tripInfo.DepartureDate,
                    FlightSegments = new List<FlightFareSegment>()
                };
                do
                {
                    fareTrip.FlightSegments.Add(MapFlightFareSegment(segments[i]));
                    if (i > 0)
                        totalTransitDuration = totalTransitDuration.Add(segments[i].DepartureDateTime - segments[i - 0].ArrivalDateTime);
                    i++;
                } while (i < segments.Count() && segments[i - 1].ArrivalAirportLocationCode != tripInfo.DestinationAirport);
                fareTrip.TotalDuration = TimeSpan.FromMinutes(segments.Sum(segment => segment.JourneyDuration)) +
                                         totalTransitDuration;
                flightTrips.Add(fareTrip);
            }
            return flightTrips;
        }

        private static FlightFareSegment MapFlightFareSegment(FlightSegment flightSegment)
        {
            List<FlightStop> stops = null;
            if (flightSegment.StopQuantity > 0)
            {
                stops = new List<FlightStop>
                    {
                        new FlightStop
                        {
                            Airport = flightSegment.StopQuantityInfo.LocationCode,
                            Arrival = flightSegment.StopQuantityInfo.ArrivalDateTime,
                            Departure = flightSegment.StopQuantityInfo.DepartureDateTime,
                            Duration = TimeSpan.FromMinutes(flightSegment.StopQuantityInfo.Duration)
                        }
                    };
            }
            var segment = new FlightFareSegment
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
