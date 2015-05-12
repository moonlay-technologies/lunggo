using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.PeerResolvers;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Interface;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Utility;
using Lunggo.ApCommon.Mystifly.OnePointService.Flight;
using PassengerType = Lunggo.ApCommon.Mystifly.OnePointService.Flight.PassengerType;

namespace Lunggo.ApCommon.Mystifly
{
    internal partial class MystiflyWrapper
    {
        internal override GetTripDetailsResult GetTripDetails(TripDetailsConditions conditions)
        {
            var request = new AirTripDetailsRQ
            {
                UniqueID = FlightIdUtil.GetCoreId(conditions.BookingId),
                SendOnlyTicketed = false,
                SessionId = Client.SessionId,
                Target = Client.Target,
                ExtensionData = null
            };
            var result = new GetTripDetailsResult();
            var retry = 0;
            var done = false;
            while (!done)
            {
                var response = Client.TripDetails(request);
                done = true;
                if (response.Success && !response.Errors.Any())
                {
                    result = MapResult(response, conditions);
                    result.IsSuccess = true;
                    result.Errors = null;
                    result.ErrorMessages = null;
                }
                else
                {
                    if (response.Errors.Any())
                    {
                        result.Errors = new List<FlightError>();
                        result.ErrorMessages = new List<string>();
                        foreach (var error in response.Errors)
                        {
                            if (error.Code == "ERTDT002")
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

        private static GetTripDetailsResult MapResult(AirTripDetailsRS response, ConditionsBase conditions)
        {
            var result = new GetTripDetailsResult();
            result.BookingId = response.TravelItinerary.UniqueID;
            result.FlightSegmentCount = response.TravelItinerary.ItineraryInfo.ReservationItems.Count();
            result.BookingNotes = response.TravelItinerary.BookingNotes.ToList();
            result.FlightItineraryDetails = new FlightItineraryDetails
            {
                FlightTrips = MapDetailsFlightTrips(response, conditions),
                PassengerInfo = MapDetailsPassengerInfo(response),
            };
            result.TotalFare =
                decimal.Parse(response.TravelItinerary.ItineraryInfo.ItineraryPricing.TotalFare.Amount);
            result.PSCFare =
                decimal.Parse(response.TravelItinerary.ItineraryInfo.ItineraryPricing.Tax.Amount);
            result.Currency = response.TravelItinerary.ItineraryInfo.ItineraryPricing.TotalFare.CurrencyCode;
            MapDetailsPTCFareBreakdowns(response, result);
            return result;
        }

        private static List<FlightTripDetails> MapDetailsFlightTrips(AirTripDetailsRS response, ConditionsBase conditions)
        {
            var flightTrips = new List<FlightTripDetails>();
            var segments = response.TravelItinerary.ItineraryInfo.ReservationItems;
            var totalTransitDuration = new TimeSpan();
            var i = 0;
            foreach (var tripInfo in conditions.TripInfos)
            {
                var fareTrip = new FlightTripDetails
                {
                    OriginAirport = tripInfo.OriginAirport,
                    DestinationAirport = tripInfo.DestinationAirport,
                    DepartureDate = tripInfo.DepartureDate,
                    FlightSegments = new List<FlightSegmentDetails>(),
                };
                do
                {
                    fareTrip.FlightSegments.Add(MapFlightSegmentDetails(segments[i]));
                    if (i > 0)
                        totalTransitDuration = totalTransitDuration.Add(segments[i].DepartureDateTime - segments[i - 0].ArrivalDateTime);
                    i++;
                } while (i < segments.Count() && segments[i - 1].ArrivalAirportLocationCode != tripInfo.DestinationAirport);
                flightTrips.Add(fareTrip);
            }
            return flightTrips;
        }

        private static FlightSegmentDetails MapFlightSegmentDetails(ReservationItem item)
        {
            return new FlightSegmentDetails
            {
                Reference = item.ItemRPH,
                DepartureAirport = item.DepartureAirportLocationCode,
                ArrivalAirport = item.ArrivalAirportLocationCode,
                DepartureTime = item.DepartureDateTime,
                ArrivalTime = item.ArrivalDateTime,
                DepartureTerminal = item.DepartureTerminal,
                ArrivalTerminal = item.ArrivalTerminal,
                Duration = TimeSpan.FromMinutes(double.Parse(item.JourneyDuration)),
                AirlineCode = item.MarketingAirlineCode,
                FlightNumber = item.FlightNumber,
                OperatingAirlineCode = item.OperatingAirlineCode,
                AircraftCode = item.AirEquipmentType,
                Rbd = item.ResBookDesigCode,
                StopQuantity = item.StopQuantity,
                Baggage = item.Baggage,
                Pnr = item.AirlinePNR
            };
        }

        private static List<PassengerInfoDetails> MapDetailsPassengerInfo(AirTripDetailsRS response)
        {
            var passengerInfoDetails = new List<PassengerInfoDetails>();
            foreach (var customerInfo in response.TravelItinerary.ItineraryInfo.CustomerInfos)
            {
                var eTicket =
                    MapETicket(customerInfo.ETickets);
                var passengerInfo = new PassengerInfoDetails
                {
                    Title = MapDetailsPassengerTitle(customerInfo),
                    FirstName = customerInfo.Customer.PaxName.PassengerFirstName,
                    LastName = customerInfo.Customer.PaxName.PassengerLastName,
                    Type = MapDetailsPassengerType(customerInfo),
                    IdNumber = customerInfo.Customer.PassportNumber,
                    ETicket = eTicket
                };
                passengerInfoDetails.Add(passengerInfo);
            }
            return passengerInfoDetails;
        }

        private static List<Eticket> MapETicket(IEnumerable<ETicket> eticket)
        {
            return eticket.Select(e => new Eticket
            {
                Reference = e.ItemRPH,
                Number = e.eTicketNumber
            }).ToList();
        }

        private static Title MapDetailsPassengerTitle(CustomerInfo customerInfo)
        {
            switch (customerInfo.Customer.PaxName.PassengerTitle)
            {
                case "Mr":
                    return Title.Mister;
                case "Mrs":
                    return Title.Mistress;
                case "Miss":
                    return Title.Miss;
                default:
                    return Title.Mister;
            }
        }

        private static Flight.Constant.PassengerType MapDetailsPassengerType(CustomerInfo customerInfo)
        {
            switch (customerInfo.Customer.PassengerType)
            {
                case PassengerType.ADT:
                    return Flight.Constant.PassengerType.Adult;
                case PassengerType.CHD:
                    return Flight.Constant.PassengerType.Child;
                case PassengerType.INF:
                    return Flight.Constant.PassengerType.Infant;
                default:
                    return Flight.Constant.PassengerType.Adult;
            }
        }

        private static void MapDetailsPTCFareBreakdowns(AirTripDetailsRS response, GetTripDetailsResult result)
        {
            foreach (var breakdown in response.TravelItinerary.ItineraryInfo.TripDetailsPTC_FareBreakdowns)
            {
                switch (breakdown.PassengerTypeQuantity.Code)
                {
                    case PassengerType.ADT:
                        result.AdultTotalFare =
                            decimal.Parse(breakdown.TripDetailsPassengerFare.TotalFare.Amount);
                        break;
                    case PassengerType.CHD:
                        result.ChildTotalFare =
                            decimal.Parse(breakdown.TripDetailsPassengerFare.TotalFare.Amount);
                        break;
                    case PassengerType.INF:
                        result.InfantTotalFare =
                            decimal.Parse(breakdown.TripDetailsPassengerFare.TotalFare.Amount);
                        break;
                }
            }
        }

        private static void MapError(AirTripDetailsRS response, ResultBase result)
        {
            foreach (var error in response.Errors)
            {
                switch (error.Code)
                {
                    case "ERTDT001":
                    case "ERTDT003":
                        goto case "InvalidInputData";
                    case "ERTDT004":
                    case "ERTDT005":
                    case "ERTDT006":
                        goto case "BookingIdNoLongerValid";
                    case "ERTDT002":
                        if (result.ErrorMessages == null)
                            result.ErrorMessages = new List<string>();
                        result.ErrorMessages.Add("Invalid account information!");
                        goto case "TechnicalError";
                    case "ERGEN006":
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
                    case "BookingIdNoLongerValid":
                        if (!result.Errors.Contains(FlightError.BookingIdNoLongerValid))
                            result.Errors.Add(FlightError.BookingIdNoLongerValid);
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
