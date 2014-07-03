using Lunggo.Flight.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.Flight.Crawler
{
    public interface ICrawler
    {
        List<FlightTicket> Search(TicketSearch SearchParam);
    }
}
