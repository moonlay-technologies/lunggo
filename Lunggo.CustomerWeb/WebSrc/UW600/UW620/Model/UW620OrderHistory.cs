using System;

namespace Lunggo.CustomerWeb.WebSrc.UW600.UW620.Model
{
    public class Uw620OrderHistory
    {
        public string Type { get; set; }
    }

    public class Uw620HotelHistory : Uw620OrderHistory
    {
        public string IdMember { get; set; }
        public string HotelName { get; set; }
        public string Address { get; set; }
        public DateTime CheckInDateTime { get; set; }
        public DateTime CheckOutDateTime { get; set; }
        public string OrderId { get; set; }
        public string StatusPayment { get; set; }
    }

    public class Uw620FlightHistory : Uw620OrderHistory
    {
        public string OrderId { get; set; }
        public string StatusPayment { get; set; }
        public string IdMember { get; set; }
        public string Airline { get; set; }
        public string TypeWay { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ReturnTime { get; set; }

    }

}
