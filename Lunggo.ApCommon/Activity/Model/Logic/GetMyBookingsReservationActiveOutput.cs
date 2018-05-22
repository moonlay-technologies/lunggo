using System;
using System.Collections.Generic;

namespace Lunggo.ApCommon.Activity.Model.Logic
{
    public class GetMyBookingsReservationActiveOutput
    {
        public List<BookingDetail> MyReservations { get; set; }
        public bool MustUpdate { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
