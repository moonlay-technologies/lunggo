using System;
using Lunggo.Framework.Database;

namespace Lunggo.BackendWeb.Query
{
    public class GetHotelBookingDetailRecord : QueryRecord
    {
        public String RsvNo { get; set; }
        public DateTime? RsvTime { get; set; }
        public String PaymentStatusCd { get; set; }
        public String HotelNo { get; set; }
        public String ContactName { get; set; }
        public String ContactEmail { get; set; }
        public String ContactPhone { get; set; }
        public String ContactAddress { get; set; }
        public String PaymentMethodCd { get; set; }
        public String FinalPrice { get; set; }

    }
}
