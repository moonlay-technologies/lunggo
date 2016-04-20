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
            var itins = GetItinerarySetFromCache(input.ItinCacheId);
            var bookResults = BookItineraries(itins, input, output);
            output.IsValid = bookResults.TrueForAll(result => result.RevalidateSet.IsValid);
            if (output.IsValid)
            {
                output.IsItineraryChanged = bookResults.Exists(result => result.RevalidateSet.IsItineraryChanged);
                var newItins = bookResults.Select(result => result.RevalidateSet.NewItinerary).ToList();
                if (output.IsItineraryChanged)
                    output.NewItinerary = ConvertToItineraryForDisplay(BundleItineraries(newItins));
                AddPriceMargin(newItins);
                output.IsPriceChanged = bookResults.Exists(result => result.RevalidateSet.IsPriceChanged);
                if (output.IsPriceChanged)
                    output.NewPrice = newItins.Sum(itin => itin.LocalPrice);
                SaveItinerarySetAndBundleToCache(newItins, BundleItineraries(newItins), input.ItinCacheId);
            }
            if (AllAreBooked(bookResults))
            {
                output.IsSuccess = true;
                var reservation = CreateReservation(itins, input, bookResults);
                InsertDb.Reservation(reservation);
                output.RsvNo = reservation.RsvNo;
                //Delete Itinerary From Cache
                DeleteItineraryFromCache(input.ItinCacheId);
                DeleteItinerarySetFromCache(input.ItinCacheId);
            }
            else
            {
                output.IsSuccess = false;
                if (AnyIsBooked(bookResults))
                    output.PartiallySucceed();
                output.DistinguishErrors();
            }

            
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

        private static bool AllAreBooked(IEnumerable<BookResult> bookResults)
        {
            return bookResults.All(set => set.IsSuccess);
        }

        private FlightReservation CreateReservation(List<FlightItinerary> itins, BookFlightInput input, List<BookResult> bookResults)
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
                Payment = new PaymentInfo(),
                TripType = ParseTripType(trips)
            };
            reservation.Payment.TimeLimit = bookResults.Min(res => res.TimeLimit);
            reservation.Payment.Status = PaymentStatus.Pending;
            return reservation;
        }

        private List<BookResult> BookItineraries(IEnumerable<FlightItinerary> itins, BookFlightInput input, BookFlightOutput output)
        {
            var bookResults = new List<BookResult>();
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
                ContactData = input.Contact,
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
                response.Errors.ForEach(output.AddError);
                if (response.ErrorMessages != null)
                    response.ErrorMessages.ForEach(output.AddError);
            }
            bookResult.RevalidateSet = new RevalidateFlightOutputSet
            {
                IsValid = response.IsValid,
                IsItineraryChanged = response.IsItineraryChanged,
                NewItinerary = response.NewItinerary,
                IsPriceChanged = response.IsPriceChanged,
                NewPrice = response.NewPrice
            };
            return bookResult;
        }
    }

        #endregion
}
