using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Campaign.Model;
using Lunggo.ApCommon.Campaign.Service;
using Lunggo.ApCommon.Flight.Constant;

using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Model.Logic;
using Lunggo.ApCommon.Payment;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Sequence;
using Lunggo.ApCommon.Voucher;
using System.Diagnostics;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        public BookFlightOutput BookFlight(BookFlightInput input)
        {
            var output = new BookFlightOutput();
            var itins = GetItinerarySetFromCache(input.Token); 
            var bookResults = BookItineraries(itins, input, output);
            output.IsValid = bookResults.TrueForAll(result => result.RevalidateSet.IsValid);
            if (output.IsValid)
            {
                output.IsItineraryChanged = bookResults.Exists(result => result.RevalidateSet.IsItineraryChanged);
                var newItins = bookResults.Select(result => result.RevalidateSet.NewItinerary).ToList();
                if (output.IsItineraryChanged)
                    output.NewItinerary = ConvertToItineraryForDisplay(BundleItineraries(newItins));
                output.IsPriceChanged = bookResults.Exists(result => result.RevalidateSet.IsPriceChanged);
                if (output.IsPriceChanged)
                    output.NewPrice = bookResults.Sum(result => result.RevalidateSet.NewPrice);
                SaveItinerarySetAndBundleToCache(newItins, BundleItineraries(newItins), input.Token);
            }
            if (AllAreBooked(bookResults))
            {
                output.IsSuccess = true;
                var reservation = CreateReservation(itins, input, output);
                InsertDb.Reservation(reservation);
                if (reservation.Payment.Method == PaymentMethod.BankTransfer)
                    SendPendingPaymentReservationNotifToCustomer(reservation.RsvNo);
                if (reservation.Payment.Method == PaymentMethod.VirtualAccount)
                    SendInstantPaymentReservationNotifToCustomer(reservation.RsvNo);
                SavePaymentRedirectionUrlInCache(reservation.RsvNo, reservation.Payment.Url, reservation.Payment.TimeLimit);
                output.RsvNo = reservation.RsvNo;
                output.PaymentUrl = reservation.Payment.Url;
                output.TimeLimit = reservation.Payment.TimeLimit;

                if (reservation.Payment.Status != PaymentStatus.Failed)
                    output.IsSuccess = true;
                else
                {
                    output.IsSuccess = false;
                    output.Errors = new List<FlightError> {FlightError.PaymentFailed};
            }
            }
            else
            {
                output.IsSuccess = false;
                if (!bookResults.Any())
                    output.AddError(FlightError.InvalidInputData);
                if (AnyIsBooked(bookResults))
                    output.PartiallySucceed();
                output.DistinguishErrors();
            }

            //Delete Itinerary From Cache
            DeleteItineraryFromCache(input.Token);
            DeleteItinerarySetFromCache(input.Token);
            return output;
        }

        public string GetBookingRedirectionUrl(string rsvNo)
        {
            return GetPaymentRedirectionUrlInCache(rsvNo);
        }

        #region Helpers

        private static bool AnyIsBooked(IEnumerable<BookResult> bookResults)
        {
            return bookResults.Any(set => set.IsSuccess);
        }

        private static bool AllAreBooked(List<BookResult> bookResults)
        {
            return bookResults.Any() && bookResults.All(set => set.IsSuccess);
        }

        private FlightReservation CreateReservation(List<FlightItinerary> itins, BookFlightInput input, BookFlightOutput output)
        {
            var trips =
                itins.SelectMany(itin => itin.Trips).OrderBy(trip => trip.Segments.First().DepartureTime).ToList();
            var reservation = new FlightReservation
            {
                RsvNo = FlightRsvNoSequence.GetInstance().GetNextFlightRsvNo(),
                RsvTime = DateTime.UtcNow,
                Itineraries = itins,
                Contact = input.Contact,
                Passengers = input.Passengers,
                TripType = ParseTripType(trips),
                Payment = new PaymentData
                {
                    FinalPrice = itins.Sum(itin => itin.LocalPrice)
                }
            };
            return reservation;
        }

        private List<ItemDetails> ConstructItemDetails(FlightReservation reservation)
        {
            var itemDetails = new List<ItemDetails>();
            var trips = reservation.Itineraries.SelectMany(itin => itin.Trips).ToList();
            var itemNameBuilder = new StringBuilder();
            foreach (var trip in trips)
            {
                itemNameBuilder.Append(trip.OriginAirport + "-" + trip.DestinationAirport);
                itemNameBuilder.Append(" " + trip.DepartureDate.ToString("dd-MM-yyyy"));
                if (trip != trips.Last())
                {
                    itemNameBuilder.Append(", ");
                }
            }
            var itemName = itemNameBuilder.ToString();
            itemDetails.Add(new ItemDetails
            {
                Id = "1",
                Name = itemName,
                Price = (long) reservation.Itineraries.Sum(itin => itin.LocalPrice),
                Quantity = 1
            });
            if (reservation.Discount.Nominal != 0)
                itemDetails.Add(new ItemDetails
                {
                    Id = "2",
                    Name = "Discount",
                    Price = (long) -reservation.Discount.Nominal,
                    Quantity = 1
                });
            return itemDetails;
        }

        private TransactionDetails ConstructTransactionDetails(FlightReservation reservation)
        {
            return new TransactionDetails
            {
                OrderId = reservation.RsvNo,
                OrderTime = reservation.RsvTime,
                Amount = (long) reservation.Payment.FinalPrice
            };
        }

        private List<BookResult> BookItineraries(IEnumerable<FlightItinerary> itins, BookFlightInput input, BookFlightOutput output)
        {
            var bookResults = new List<BookResult>();
            if (itins != null)
            Parallel.ForEach(itins, itin =>
            {
                var bookResult = BookItinerary(itin, input, output);
                bookResults.Add(bookResult);
            });
            return bookResults;
        }

        private BookResult BookItinerary(FlightItinerary itin, BookFlightInput input, BookFlightOutput output)
        {
            var bookInfo = new FlightBookingInfo
            {
                Itinerary = itin,
                Contact = input.Contact,
                Passengers = input.Passengers
            };
            var response = BookFlightInternal(bookInfo);
            var bookResult = new BookResult();
            if (response.IsSuccess)
            {
                bookResult.IsSuccess = true;
                itin.BookingId = response.Status.BookingId;
                itin.BookingStatus = response.Status.BookingStatus;
                if (response.Status.BookingStatus == BookingStatus.Booked)
                {
                    bookResult.TimeLimit = response.Status.TimeLimit;
                    itin.TicketTimeLimit = bookResult.TimeLimit;
                }
            }
            else
            {
                bookResult.IsSuccess = false;
                itin.BookingId = response.Status.BookingId;
                response.Errors.ForEach(output.AddError);
                if (response.ErrorMessages != null)
                    response.ErrorMessages.ForEach(output.AddError);
            }
            bookResult.RevalidateSet.IsValid = response.IsValid;
            bookResult.RevalidateSet.IsItineraryChanged = response.IsItineraryChanged;
            bookResult.RevalidateSet.NewItinerary = response.NewItinerary;
            bookResult.RevalidateSet.IsPriceChanged = response.IsPriceChanged;
            bookResult.RevalidateSet.NewPrice = response.NewPrice;
            return bookResult;
        }
    }

        #endregion
}
