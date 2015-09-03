using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Flight.Constant;

using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Model.Logic;
using Lunggo.ApCommon.Sequence;
using Lunggo.ApCommon.Voucher;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;
using Lunggo.Repository.TableRepository;

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
                InsertFlightDb.Reservation(reservation);
                output.RsvNo = reservation.RsvNo;
                output.FinalPrice = reservation.Payment.FinalPrice;
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
                itins.SelectMany(itin => itin.FlightTrips).OrderBy(trip => trip.FlightSegments.First().DepartureTime).ToList();
            var reservation = new FlightReservation
            {
                RsvNo =
                    FlightReservationSequence.GetInstance()
                        .GetFlightReservationId(EnumReservationType.ReservationType.NonMember),
                RsvTime = DateTime.UtcNow,
                Itineraries = itins,
                Contact = input.Contact,
                Passengers = input.Passengers,
                Payment = input.Payment,
                TripType = ParseTripType(trips)
            };
            reservation.Payment.FinalPrice = reservation.Itineraries.Sum(itin => itin.LocalPrice);
            reservation.Payment.TimeLimit = output.BookResults.Min(res => res.TimeLimit);
            var discountRuleIds = VoucherService.GetInstance()
                .GetFlightDiscountRules(input.DiscountCode, input.Contact.Email);
            var discountRule = GetMatchingDiscountRule(discountRuleIds) ??
                               new DiscountRule
                               {
                                   Coefficient = 0,
                                   Constant = 0
                               };
            var discountNominal = reservation.Payment.FinalPrice*discountRule.Coefficient +
                                  discountRule.Constant;
            reservation.Payment.FinalPrice -= discountNominal;
            reservation.Discount = new DiscountData
            {
                Code = input.DiscountCode,
                Id = discountRule.RuleId,
                Coefficient = discountRule.Coefficient,
                Constant = discountRule.Constant,
                Nominal = discountNominal
            };
            return reservation;
        }

        private List<BookResult> BookItineraries(IEnumerable<FlightItinerary> itins, BookFlightInput input, BookFlightOutput output)
        {
            return itins.Select(itin => BookItinerary(itin, input, output)).ToList();
        }

        private BookResult BookItinerary(FlightItinerary itin, BookFlightInput input, BookFlightOutput output)
        {
            var bookInfo = new FlightBookingInfo
            {
                FareId = itin.FareId,
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
                    bookResult.TimeLimit = response.Status.TimeLimit;
            }
            else
            {
                bookResult.IsSuccess = false;
                itin.BookingId = response.Status.BookingId;
                foreach (var error in response.Errors)
                {
                    output.AddError(error);
                }
                foreach (var errorMessage in response.ErrorMessages)
                {
                    output.AddError(errorMessage);
                }
            }
            return bookResult;
        }
    }

        #endregion
}
