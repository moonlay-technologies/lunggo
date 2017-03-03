using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Hotel.Model;

namespace Lunggo.ApCommon.Flight.Model
{
    public class HotelBookingNotif
    {
        public string CompanyId { get; set; }
        public List<HotelReservationForDisplay> Reservation { get; set; }
    }
}
