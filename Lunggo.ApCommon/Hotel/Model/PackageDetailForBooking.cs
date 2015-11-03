using System;
using System.Collections.Generic;

namespace Lunggo.ApCommon.Hotel.Model
{
    public class PackageDetailForBooking
    {
        public String PackageId { get; set; }
        public IEnumerable<RoomDetailForBooking> Rooms { get; set; }
        public Decimal FinalPriceFromSupplier { get; set; }
    }
}
