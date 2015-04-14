using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.Framework.Payment.Data;

namespace Lunggo.ApCommon.Flight.Query.Model
{
    internal class FlightRecord
    {
        internal List<PassengerFareInfo> Passengers { get; set; }
        internal List<ItineraryRecord> ItineraryRecords { get; set; }
        internal ContactData ContactData { get; set; }
        internal PaymentData PaymentData { get; set; }
    }

    internal class ItineraryRecord
    {
        internal FlightFareItinerary Itinerary { get; set; }
        internal BookResult BookResult { get; set; }
        internal List<OriginDestinationInfo> OriginDestinationInfos { get; set; }
    }
}
