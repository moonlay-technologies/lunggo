using System.Collections.Generic;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Payment.Model;
using System;

namespace Lunggo.ApCommon.Flight.Model.Logic
{
    public class BookFlightInput
    {
        public string ItinCacheId { get; set; }
        public TripType OverallTripType { get; set; }
        public List<FlightPassenger> Passengers { get; set; }
        public ContactData Contact { get; set; }
        public PaymentInfo Payment { get; set; }
        public string DiscountCode { get; set; }
        public string TransferToken { get; set; }
    }
}
