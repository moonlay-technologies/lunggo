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
        public DateAndSession DateTime { get; set; }
        public List<Pax> Passengers { get; set; }
        public Contact Contact { get; set; }
        public long PackageId { get; set; }
        public List<ActivityPricePackageReservation> TicketCount { get; set; }
    }
}
