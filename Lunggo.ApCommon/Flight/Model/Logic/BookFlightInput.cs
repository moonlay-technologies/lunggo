using System.Collections.Generic;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.Framework.Payment.Data;

namespace Lunggo.ApCommon.Flight.Model.Logic
{
    public class BookFlightInput
    {
        public TripType OverallTripType { get; set; }
        public FlightBookingInfo BookingInfo { get; set; }
        public FlightItineraryFare Itinerary { get; set; }
        public List<TripInfo> TripInfos { get; set; }
        public PaymentData PaymentData { get; set; }
    }
}
