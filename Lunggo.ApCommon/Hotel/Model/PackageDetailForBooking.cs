using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Hotel.Model
{
    public class PackageDetailForBooking
    {
        public String PackageId { get; set; }
        public IEnumerable<RoomDetailForBooking> Rooms { get; set; }
        public Decimal FinalPriceFromSupplier { get; set; }
    }
}
