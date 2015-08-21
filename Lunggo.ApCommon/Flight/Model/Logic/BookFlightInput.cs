using System.Collections.Generic;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.Framework.Payment.Data;

namespace Lunggo.ApCommon.Flight.Model.Logic
{
    public class BookFlightInput
    {
        public FlightItinerary Itinerary { get; set; }
        public List<FlightTrip> Trips { get; set; }
        public TripType OverallTripType { get; set; }
        public List<PassengerInfoFare> PassengerInfoFares { get; set; }
        public ContactData ContactData { get; set; }
        public string DiscountCode { get; set; }
    }
}
