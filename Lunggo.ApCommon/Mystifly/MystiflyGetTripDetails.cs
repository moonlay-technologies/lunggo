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
    public partial class MystiflyWrapper : IGetTripDetails
    {
        public GetTripDetailsResult GetTripDetails(FlightBooking booking)
        {
            using (var client = new MystiflyClientHandler())
            {
                var request = new AirTripDetailsRQ
                {
                    UniqueID = booking.BookingId,
                    SendOnlyTicketed = false,
                    SessionId = client.SessionId,
                    Target = client.Target,
                    ExtensionData = null
                };
                var result = new GetTripDetailsResult();
                var retry = 0;
                var done = false;
                while (retry < 3 && !done)
                {
                    done = true;
                    var response = client.TripDetails(request);
                    if (!response.Errors.Any())
                    {
                        result = MapResult(response);
                        result.Success = true;
                    }
                    else
                    {
                        foreach (var error in response.Errors)
                        {
                            if (error.Code == "ERTDT002")
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

        private static GetTripDetailsResult MapResult(AirTripDetailsRS response)
        {
            var result = new GetTripDetailsResult();
            result.BookingId = response.TravelItinerary.UniqueID;
            result.FlightSegmentCount = response.TravelItinerary.ItineraryInfo.ReservationItems.Count();
            result.BookingNotes = response.TravelItinerary.BookingNotes.ToList();
            result.FlightItineraryDetails.FlightTrips = MapDetailsFlightTrips(response);
            result.FlightItineraryDetails.PassengerInfo = MapDetailsPassengerInfo(response);
            result.TotalFare =
                decimal.Parse(response.TravelItinerary.ItineraryInfo.ItineraryPricing.TotalFare.Amount);
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
                    DepartureTime = reservationItem.DepartureDateTime,
                    ArrivalTime = reservationItem.ArrivalDateTime,
                    Duration = int.Parse(reservationItem.JourneyDuration),
                    DepartureAirport = reservationItem.DepartureAirportLocationCode,
                    DepartureTerminal = reservationItem.DepartureTerminal,
                    ArrivalAirport = reservationItem.ArrivalAirportLocationCode,
                    ArrivalTerminal = reservationItem.ArrivalTerminal,
                    AirlineCode = reservationItem.OperatingAirlineCode,
                    FlightNumber = reservationItem.FlightNumber,
                    RBD = reservationItem.ResBookDesigCode,
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
    }
}
