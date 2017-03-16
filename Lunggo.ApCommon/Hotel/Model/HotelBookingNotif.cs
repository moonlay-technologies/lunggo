using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Hotel.Model;

namespace Lunggo.ApCommon.Hotel.Model
{
    public class HotelBookingNotif
    {
        public string Token { get; set; }
        public HotelReservationForDisplay Reservation { get; set; }
    }
}
