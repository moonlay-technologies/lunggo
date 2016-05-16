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
using Lunggo.ApCommon.ProductBase.Constant;
using Lunggo.ApCommon.ProductBase.Model;
using Lunggo.ApCommon.Sequence;
using Lunggo.ApCommon.Voucher;
using System.Diagnostics;
using Lunggo.Framework.Config;
using Lunggo.Framework.Context;

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        public BookFlightOutput BookFlight(BookFlightInput input)
        {
            var output = new BookFlightOutput();
            var itins = GetItinerariesFromCache(input.Token);
            var bookResults = BookItineraries(itins, input, output);
            output.IsValid = bookResults.TrueForAll(result => result.RevalidateSet.IsValid);
            if (output.IsValid)
            {
                output.IsItineraryChanged = bookResults.Exists(result => result.RevalidateSet.IsItineraryChanged);
                var newItins = bookResults.Select(result => result.RevalidateSet.NewItinerary).ToList();
                if (output.IsItineraryChanged)
                    output.NewItinerary = ConvertToItineraryForDisplay(newItins);
                output.IsPriceChanged = bookResults.Exists(result => result.RevalidateSet.IsPriceChanged);
                if (output.IsPriceChanged)
                    output.NewPrice = bookResults.Sum(result => result.RevalidateSet.NewPrice);
                SaveItinerariesToCache(newItins, input.Token);
            }
            if (AllAreBooked(bookResults))
            {
                output.IsSuccess = true;
                var reservation = CreateReservation(itins, input, bookResults);
                InsertReservationToDb(reservation);
                output.RsvNo = reservation.RsvNo;
                
                DeleteItinerariesFromCache(input.Token);
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

        private FlightReservation CreateReservation(List<FlightItinerary> itins, BookFlightInput input, List<BookResult> bookResults)
        {
            var a = new FlightReservation();

            var trips =
                itins.SelectMany(itin => itin.Trips).OrderBy(trip => trip.Segments.First().DepartureTime).ToList();
            var reservation = new FlightReservation();
            reservation.RsvNo = RsvNoSequence.GetInstance().GetNext(reservation.Type);
            reservation.RsvTime = DateTime.UtcNow;
            reservation.RsvStatus = RsvStatus.Pending;
            reservation.Orders = itins;
            reservation.Contact = input.Contact;
            reservation.Passengers = input.Passengers;
            reservation.OverallTripType = ParseTripType(trips);
            reservation.Payment = new PaymentDetails
            {
                Status = PaymentStatus.Pending,
                LocalCurrency = new Currency(OnlineContext.GetActiveCurrencyCode()),
                OriginalPriceIdr = reservation.Orders.Sum(order => order.Price.FinalIdr),
                TimeLimit = reservation.Orders.Min(order => order.TimeLimit.GetValueOrDefault()).AddMinutes(-30),
            };
            return reservation;
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
                    itin.TimeLimit = bookResult.TimeLimit;
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

        public BookFlightResult BookFlightInternal(FlightBookingInfo bookInfo)
        {
            var fareType = IdUtil.GetFareType(bookInfo.Itinerary.FareId);
            var supplierName = IdUtil.GetSupplier(bookInfo.Itinerary.FareId);
            bookInfo.Itinerary.FareId = IdUtil.GetCoreId(bookInfo.Itinerary.FareId);
            var supplier = Suppliers.Where(entry => entry.Value.SupplierName == supplierName).Select(entry => entry.Value).Single();
            var result = supplier.BookFlight(bookInfo);
            if (result.Status != null && result.Status.BookingId != null)
                result.Status.BookingId = IdUtil.ConstructIntegratedId(result.Status.BookingId,
                    supplierName, fareType);
            var defaultTimeout = DateTime.UtcNow.AddMinutes(double.Parse(ConfigManager.GetInstance().GetConfigValue("flight", "paymentTimeout")));
            if (result.Status != null)
                result.Status.TimeLimit = defaultTimeout < result.Status.TimeLimit
                    ? defaultTimeout
                    : result.Status.TimeLimit;
            if (result.NewItinerary != null)
                result.NewItinerary.FareId = IdUtil.ConstructIntegratedId(result.NewItinerary.FareId, supplierName, result.NewItinerary.FareType);
            return result;
        }
    }

        #endregion
}
