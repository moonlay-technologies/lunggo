using System;
using System.Collections.Generic;
using System.Linq;
using Lunggo.ApCommon.Constant;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.ApCommon.Product.Model;

namespace Lunggo.ApCommon.Flight.Wrapper
{
    internal abstract class FlightSupplierWrapperBase
    {
        internal abstract Supplier SupplierName { get; }

        internal abstract void Init();
        internal abstract SearchFlightResult SearchFlight(SearchFlightConditions conditions);
        internal abstract RevalidateFareResult RevalidateFare(RevalidateConditions conditions);
        internal abstract BookFlightResult BookFlight(FlightBookingInfo bookInfo);
        internal abstract IssueTicketResult OrderTicket(string bookingId, bool canHold);
        internal abstract GetTripDetailsResult GetTripDetails(TripDetailsConditions conditions);
        internal abstract Currency CurrencyGetter(string currencyName);
        internal abstract decimal GetDeposit();
        internal abstract List<BookingStatusInfo> GetBookingStatus();

        protected static void CommonInputCheck(List<Pax> passengers, DateTime departureDate, out List<string> messages)
        {
            messages = new List<string>();

            var infants = passengers.Where(pax => pax.Type == PaxType.Infant).ToList();
            var children = passengers.Where(pax => pax.Type == PaxType.Child).ToList();
            var isInfantValid = infants.All(inft => inft.DateOfBirth.GetValueOrDefault().AddMonths(2) <= departureDate && inft.DateOfBirth.GetValueOrDefault().AddYears(2) > departureDate);
            var isChildValid = children.All(child => child.DateOfBirth.GetValueOrDefault().AddYears(2) <= departureDate && child.DateOfBirth.GetValueOrDefault().AddYears(12) > departureDate);

            var adultCount = passengers.Count(pax => pax.Type == PaxType.Adult);
            var childCount = children.Count;
            var infantCount = infants.Count;

            if (adultCount == 0)
                messages.Add("There Must be one adult");

            if (adultCount + childCount > 7)
                messages.Add("Total adult and children passenger must be not more than seven");

            if (adultCount < infantCount)
                messages.Add("Each infant must be accompanied by one adult");

            if (departureDate > DateTime.Now.AddDays(331).Date)
                messages.Add("Time of Departure Exceeds");

            if (!isInfantValid)
                messages.Add("Age of infant when traveling must be less than 2 years old and more than or equal 2 months old");

            if (!isChildValid)
                messages.Add("Age of child when traveling must be between 2 and 12 years old");
        }
    }
}
