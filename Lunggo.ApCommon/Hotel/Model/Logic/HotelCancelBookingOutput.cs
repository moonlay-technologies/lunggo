using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Hotel.Model.Logic
{
    public class HotelCancelBookingOutput :ResultBase
    {
        public string BookingId { get; set; }
        public DateTime CancellationDate { get; set; }
        public decimal CancellationAmount { get; set; }
    }
}
