using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.PeerResolvers;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Interface;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Mystifly.OnePointService.Flight;
using PassengerType = Lunggo.ApCommon.Mystifly.OnePointService.Flight.PassengerType;

namespace Lunggo.ApCommon.Mystifly
{
    internal partial class MystiflyWrapper
    {
        internal override GetTripDetailsResult GetTripDetails(string bookingId)
        {
            using (var client = new MystiflyClientHandler())
            {
                var request = new AirTripDetailsRQ
                {
                    UniqueID = bookingId,
                    SendOnlyTicketed = false,
                    SessionId = client.SessionId,
                    Target = MystiflyClientHandler.Target,
                    ExtensionData = null
                };
                var result = new GetTripDetailsResult();
                var retry = 0;
                var done = false;
                while (!done)
                {
                    done = true;
                    var response = client.TripDetails(request);
                    if (!response.Errors.Any() && response.Success)
                    {
                        result = MapResult(response);
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
                                if (error.Code == "ERTDT002")
                                {
                                    result.Errors = null;
                                    result.ErrorMessages = null;
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
                        }
                        result.IsSuccess = false;
                    }
                }
                return result;
            }
        }

        private static GetTripDetailsResult MapResult(AirTripDetailsRS response)
        {
            var result = new GetTripDetailsResult();
            result.BookingId = response.TravelItinerary.UniqueID;
            result.FlightSegmentCount = response.TravelItinerary.ItineraryInfo.ReservationItems.Count();
            result.BookingNotes = response.TravelItinerary.BookingNotes.ToList();
            result.FlightItineraryDetails = new FlightItineraryDetails
            {
                FlightTrips = MapDetailsFlightTrips(response),
                PassengerInfo = MapDetailsPassengerInfo(response),
                Source = FlightSource.Wholesaler
            };
            result.TotalFare =
                decimal.Parse(response.TravelItinerary.ItineraryInfo.ItineraryPricing.TotalFare.Amount);
            result.PSCFare =
                decimal.Parse(response.TravelItinerary.ItineraryInfo.ItineraryPricing.Tax.Amount);
            result.Currency = response.TravelItinerary.ItineraryInfo.ItineraryPricing.TotalFare.CurrencyCode;
            MapDetailsPTCFareBreakdowns(response, result);
            return result;
        }

        private static Dictionary<int, FlightTripDetails> MapDetailsFlightTrips(AirTripDetailsRS response)
        {
            var flightTripDetails = new Dictionary<int, FlightTripDetails>();
            foreach (var reservationItem in response.TravelItinerary.ItineraryInfo.ReservationItems)
            {
                var flightTrip = new FlightTripDetails
                {
                    Pnr = reservationItem.AirlinePNR,
                    DepartureTime = reservationItem.DepartureDateTime,
                    ArrivalTime = reservationItem.ArrivalDateTime,
                    Duration = int.Parse(reservationItem.JourneyDuration),
                    DepartureAirport = reservationItem.DepartureAirportLocationCode,
                    DepartureTerminal = reservationItem.DepartureTerminal,
                    ArrivalAirport = reservationItem.ArrivalAirportLocationCode,
                    ArrivalTerminal = reservationItem.ArrivalTerminal,
                    AirlineCode = reservationItem.MarketingAirlineCode,
                    FlightNumber = reservationItem.FlightNumber,
                    OperatingAirlineCode = reservationItem.OperatingAirlineCode,
                    AircraftCode = reservationItem.AirEquipmentType,
                    Rbd = reservationItem.ResBookDesigCode,
                    Baggage = reservationItem.Baggage,
                    StopQuantity = reservationItem.StopQuantity
                };
                flightTripDetails.Add(reservationItem.ItemRPH, flightTrip);
            }
            return flightTripDetails;
        }

        private static List<PassengerInfoDetails> MapDetailsPassengerInfo(AirTripDetailsRS response)
        {
            var passengerInfoDetails = new List<PassengerInfoDetails>();
            foreach (var customerInfo in response.TravelItinerary.ItineraryInfo.CustomerInfos)
            {
                var eTicketNumbers =
                    customerInfo.ETickets.ToDictionary(
                        eTicket => eTicket.ItemRPH,
                        eTicket => eTicket.eTicketNumber);
                var passengerInfo = new PassengerInfoDetails
                {
                    Title = MapDetailsPassengerTitle(customerInfo),
                    FirstName = customerInfo.Customer.PaxName.PassengerFirstName,
                    LastName = customerInfo.Customer.PaxName.PassengerLastName,
                    Type = MapDetailsPassengerType(customerInfo),
                    PassportOrIdNumber = customerInfo.Customer.PassportNumber,
                    ETicketNumbers = eTicketNumbers
                };
                passengerInfoDetails.Add(passengerInfo);
            }
            return passengerInfoDetails;
        }

        private static Title MapDetailsPassengerTitle(CustomerInfo customerInfo)
        {
            switch (customerInfo.Customer.PaxName.PassengerTitle)
            {
                case "Mr" :
                    return Title.Mister;
                case "Mrs" :
                    return Title.Mistress;
                case "Miss" :
                    return Title.Miss;
                default :
                    return Title.Mister;
            }
        }

        private static Flight.Constant.PassengerType MapDetailsPassengerType(CustomerInfo customerInfo)
        {
            switch (customerInfo.Customer.PassengerType)
            {
                case PassengerType.ADT :
                    return Flight.Constant.PassengerType.Adult;
                case PassengerType.CHD :
                    return Flight.Constant.PassengerType.Child;
                case PassengerType.INF :
                    return Flight.Constant.PassengerType.Infant;
                default :
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
                    case "ERGEN006":
                        result.ErrorMessages.Add("Unexpected error on the other end!");
                        goto case "TechnicalError";
                    case "ERMAI001":
                        result.ErrorMessages.Add("Mystifly is under maintenance!");
                        goto case "TechnicalError";

                    case "InvalidInputData":
                        result.Errors.Add(FlightError.InvalidInputData);
                        break;
                    case "BookingIdNoLongerValid":
                        result.Errors.Add(FlightError.BookingIdNoLongerValid);
                        break;
                    case "TechnicalError":
                        result.Errors.Add(FlightError.TechnicalError);
                        break;
                }
            }
        }
    }
}
