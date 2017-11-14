using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Product.Model;

namespace Lunggo.ApCommon.Activity.Model.Logic
{
    public class BookActivityInput
    {
        public string ActivityId { get; set; }
        public DateAndAvailableHour DateTime { get; set; }
        public List<Pax> Passengers { get; set; }
        public Contact Contact { get; set; }
        public int? TicketCount { get; set; }
    }
}
