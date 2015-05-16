using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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