using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.Flight.Model
{
    public class FlightBookingDetail
    {
        public int BookingNumber { get; set; }
        public DateTime BookingDate { get; set; }
        public bool IsReturning { get; set; }
        public string OriginAirport { get; set; }
        public string DestinationAirport { get; set; }
        public string DepartureAirline { get; set; }
        public string ReturnAirline { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime ArrivalDate { get; set; }
        public FlightTicket DepartureTicket { get; set; }
        public FlightTicket ReturnTicket { get; set; }
        public List<string> PassengerName { get; set; }
        public string BookerName { get; set; }
        public decimal Price { get; set; }
        public bool IsPaid { get; set; }
    }
}
