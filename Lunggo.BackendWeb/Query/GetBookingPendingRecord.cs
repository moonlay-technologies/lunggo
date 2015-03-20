using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Lunggo.Framework.Database;

namespace Lunggo.BackendWeb.Query
{
    public class GetBookingPendingRecord : QueryRecord
    {
        public String RsvNo { get; set; }
        public DateTime? RsvTime { get; set; }
        public String ContactName  { get; set; }
        public String PaymentStatusCd { get; set; }
        public String GuestName { get; set; }
        public String HotelNo  { get; set; }
        public DateTime? CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }
        public String RsvStatusCd { get; set; }
        public decimal FinalPrice { get; set; }
        public String PaymentMethodCd { get; set; }
        public String rdSelection { get; set; }
        public String Type { get; set; }
    }
}
