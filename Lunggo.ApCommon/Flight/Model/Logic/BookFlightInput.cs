using System.Collections.Generic;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Payment.Model;

namespace Lunggo.ApCommon.Flight.Model.Logic
{
    public class BookFlightInput
    {
        public string ItinCacheId { get; set; }
        public TripType OverallTripType { get; set; }
        public List<FlightPassenger> Passengers { get; set; }
        public Contact Contact { get; set; }
        public Payment.Model.PaymentData PaymentData { get; set; }
        public string DiscountCode { get; set; }
    }
}
