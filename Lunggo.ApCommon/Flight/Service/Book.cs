using System;
using System.Collections.Generic;
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

namespace Lunggo.ApCommon.Flight.Service
{
    public partial class FlightService
    {
        public BookFlightOutput BookFlight(BookFlightInput input)
        {
            var output = new BookFlightOutput();
            var itins = GetItinerarySetFromCache(input.ItinCacheId);
            output.BookResults = BookItineraries(itins, input, output);
            if (AllAreBooked(output.BookResults))
            {
                output.IsSuccess = true;
                var reservation = CreateReservation(itins, input, output);
                InsertDb.Reservation(reservation);
                if (reservation.Payment.Method == PaymentMethod.BankTransfer)
                    SendPendingPaymentReservationNotifToCustomer(reservation.RsvNo);
                else
                    SendInstantPaymentReservationNotifToCustomer(reservation.RsvNo);
                SavePaymentRedirectionUrlInCache(reservation.RsvNo, reservation.Payment.Url, reservation.Payment.TimeLimit);
                output.RsvNo = reservation.RsvNo;
            }
            else
            {
                output.IsSuccess = false;
                if (AnyIsBooked(output.BookResults))
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
                Payment = input.Payment,
                TripType = ParseTripType(trips)
            };
            reservation.Payment.Medium = PaymentService.GetInstance().GetPaymentMedium(input.Payment.Method);
            reservation.Payment.TimeLimit = output.BookResults.Min(res => res.TimeLimit);
            var originalPrice = reservation.Itineraries.Sum(itin => itin.LocalPrice);
            var campaign = CampaignService.GetInstance().ValidateVoucherRequest(new VoucherRequest
            {
                Email = input.Contact.Email,
                Price = originalPrice,
                VoucherCode = input.DiscountCode
            });
            reservation.Payment.FinalPrice = campaign.DiscountedPrice;
            reservation.Discount = new DiscountData
            {
                Code = input.DiscountCode,
                Id = campaign.CampaignVoucher.CampaignId.GetValueOrDefault(),
                Percentage = campaign.CampaignVoucher.ValuePercentage.GetValueOrDefault(),
                Constant = campaign.CampaignVoucher.ValueConstant.GetValueOrDefault(),
                Nominal = campaign.TotalDiscount
            };
            var transactionDetails = ConstructTransactionDetails(reservation);
            var itemDetails = ConstructItemDetails(reservation);
            reservation.Payment.Url = PaymentService.GetInstance()
                .GetPaymentUrl(transactionDetails, itemDetails, reservation.Payment.Method);
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
                Price = reservation.Itineraries.Sum(itin => itin.LocalPrice),
                Quantity = 1
            });
            if (reservation.Discount.Nominal != 0)
                itemDetails.Add(new ItemDetails
                {
                    Id = "2",
                    Name = "Discount",
                    Price = -reservation.Discount.Nominal,
                    Quantity = 1
                });
            return itemDetails;
        }

        private TransactionDetails ConstructTransactionDetails(FlightReservation reservation)
        {
            return new TransactionDetails
            {
                OrderId = reservation.RsvNo,
                Amount = reservation.Payment.FinalPrice
            };
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
                FareId = itin.FareId,
                CanHold = itin.CanHold,
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
                itin.BookingId = response.Status.BookingId;
                response.Errors.ForEach(output.AddError);
                if (response.ErrorMessages != null)
                    response.ErrorMessages.ForEach(output.AddError);
            }
            return bookResult;
        }
    }

        #endregion
}
