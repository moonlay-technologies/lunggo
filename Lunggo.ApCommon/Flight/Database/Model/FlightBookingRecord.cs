using System.Collections.Generic;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Model.Logic;

namespace Lunggo.ApCommon.Flight.Database.Model
{
    internal class FlightBookingRecord
    {
        internal TripType OverallTripType { get; set; }
        internal List<PassengerInfoFare> Passengers { get; set; }
        internal List<FlightBookingItineraryRecord> ItineraryRecords { get; set; }
        internal ContactData ContactData { get; set; }
        internal string DiscountCode { get; set; }
    }

    internal class FlightBookingItineraryRecord
    {
        internal FlightItineraryFare Itinerary { get; set; }
        internal BookResult BookResult { get; set; }
    }
}
