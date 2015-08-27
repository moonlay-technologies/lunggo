using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Database.Logic;
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
            foreach (var itin in itins)
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
                output.BookResults.Add(bookResult);
            }
            if (output.BookResults.TrueForAll(set => set.IsSuccess))
            {
                output.IsSuccess = true;
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
                    TripType = input.OverallTripType
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
                var discountNominal = reservation.Payment.FinalPrice * discountRule.Coefficient +
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
            }
            else
            {
                if (output.BookResults.Any(set => set.IsSuccess))
                    output.PartiallySucceed();
                output.IsSuccess = false;
                output.Errors = output.Errors.Distinct().ToList();
                output.ErrorMessages = output.ErrorMessages.Distinct().ToList();
            }
            return output;
        }
    }
}
