using System;
using Lunggo.ApCommon.Flight.Constant;
using Lunggo.ApCommon.Flight.Model;

namespace Lunggo.ApCommon.Flight.Model
{
    internal class BookFlightResult : ResultBase
    {
        internal BookingStatusInfo Status { get; set; }
    }
}
