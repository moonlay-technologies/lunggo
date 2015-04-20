using System.Collections.Generic;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.Framework.Payment.Data;

namespace Lunggo.ApCommon.Flight.Query.Model
{
    internal class FlightBookingRecord
    {
        internal TripType OverallTripType { get; set; }
        internal List<PassengerFareInfo> Passengers { get; set; }
        internal List<FlightBookingItineraryRecord> ItineraryRecords { get; set; }
        internal ContactData ContactData { get; set; }
        internal PaymentData PaymentData { get; set; }
    }

    internal class FlightBookingItineraryRecord
    {
        internal FlightFareItinerary Itinerary { get; set; }
        internal BookResult BookResult { get; set; }
    }
}
