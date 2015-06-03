using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Payment.Model;

namespace Lunggo.ApCommon.Flight.Model
{
    public class FlightSummary
    {
        public string RsvNo { get; set; }
        public decimal TotalFare { get; set; }
        public List<PassengerInfoFare> Passengers { get; set; }
        public FlightItineraryApi Itinerary { get; set; }
        public PaymentInfo PaymentInfo { get; set; }
    }

    public class FlightSummaryDetails
    {
        public string RsvNo { get; set; }
        public List<PassengerInfoDetails> Passengers { get; set; }
        public FlightItineraryDetails Itinerary { get; set; }
        public PaymentInfo PaymentInfo { get; set; }
    }
}
