using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.Framework.Payment.Data;

namespace Lunggo.ApCommon.Flight.Model
{
    public class BookFlightInput
    {
        public FlightBookingInfo BookingInfo { get; set; }
        public FlightFareItinerary Itinerary { get; set; }
        public List<OriginDestinationInfo> OriginDestinationInfos { get; set; }
        public PaymentData PaymentData { get; set; }
    }
}
