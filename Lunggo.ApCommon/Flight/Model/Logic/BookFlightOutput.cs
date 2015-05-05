using System;
using Lunggo.ApCommon.Flight.Constant;

namespace Lunggo.ApCommon.Flight.Model.Logic
{
    public class BookFlightOutput : OutputBase
    {
        public BookResult BookResult { get; set; }
        public ReservationDetails ReservationDetails { get; set; }
    }

    public class BookResult
    {
        public BookingStatus BookingStatus { get; set; }
        public string BookingId { get; set; }
        public DateTime? TimeLimit { get; set; }
    }


    public class ReservationDetails
    {
        public string RsvNo { get; set; }
    }
}
