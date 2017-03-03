using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Flight.Model
{
    public class FlightBookingNotif
    {
        public string CompanyId { get; set; }
        public List<FlightReservationForDisplay> Reservation { get; set; }
    }
}
