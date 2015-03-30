using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Model;

namespace Lunggo.ApCommon.Flight.Interface
{
    public interface IOrderTicket
    {
        OrderTicketResult OrderTicket(FlightBooking booking);
    }
}
