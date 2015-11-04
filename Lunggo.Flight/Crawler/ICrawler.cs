using System.Collections.Generic;
using Lunggo.Flight.Model;

namespace Lunggo.Flight.Crawler
{
    public interface ICrawler
    {
        List<FlightTicket> Search(TicketSearch SearchParam);
    }
}
