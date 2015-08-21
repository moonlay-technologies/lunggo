using System.Collections.Generic;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Payment.Model;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Flight.Model
{
    public class FlightReservation
    {
        public string RsvNo { get; set; }
        public DateTime RsvTime { get; set; }
        public FlightItinerary Itinerary { get; set; }
        public List<PassengerInfoDetails> Passengers { get; set; }
        public PaymentInfo Payment { get; set; }
        public ContactData Contact { get; set; }
        public string InvoiceNo { get; set; }
        public TripType TripType { get; set; }
    }
}
