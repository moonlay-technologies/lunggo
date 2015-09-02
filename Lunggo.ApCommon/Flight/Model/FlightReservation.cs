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
    public class FlightReservationForDisplay
    {
        public string RsvNo { get; set; }
        public DateTime RsvTime { get; set; }
        public FlightItineraryForDisplay Itinerary { get; set; }
        public List<FlightPassenger> Passengers { get; set; }
        public PaymentInfo Payment { get; set; }
        public ContactData Contact { get; set; }
        public string InvoiceNo { get; set; }
        public TripType TripType { get; set; }
    }

    public class FlightReservation
    {
        public string RsvNo { get; set; }
        public DateTime RsvTime { get; set; }
        public List<FlightItinerary> Itineraries { get; set; }
        public List<FlightPassenger> Passengers { get; set; }
        public PaymentInfo Payment { get; set; }
        public ContactData Contact { get; set; }
        public string InvoiceNo { get; set; }
        public TripType TripType { get; set; }
        public DiscountData Discount { get; set; }
    }
}
