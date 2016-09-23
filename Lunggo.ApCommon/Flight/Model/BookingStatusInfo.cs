using System;
using Lunggo.ApCommon.Flight.Constant;

namespace Lunggo.ApCommon.Flight.Model
{
    public class BookingStatusInfo
    {
        public BookingStatus BookingStatus { get; set; }
        public string BookingId { get; set; }
        public DateTime TimeLimit { get; set; }
    }
}