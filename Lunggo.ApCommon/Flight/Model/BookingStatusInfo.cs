using System;
using Lunggo.ApCommon.Flight.Constant;

namespace Lunggo.ApCommon.Flight.Model
{
    internal class BookingStatusInfo
    {
        internal BookingStatus BookingStatus { get; set; }
        internal string BookingId { get; set; }
        internal DateTime? TimeLimit { get; set; }
    }
}