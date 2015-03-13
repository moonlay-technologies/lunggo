using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.Framework.Database;

namespace Lunggo.BackendWeb.Query
{
    public class GetHotelBookingDetailRecord : QueryRecord
    {
        public String RsvNo { get; set; }
        public DateTime? RsvTime { get; set; }
        public String PaymentStatusCd { get; set; }
        public String MemberCd { get; set; }
        public String HotelNo { get; set; }
        public String PaymentMethodCd { get; set; }
        public String FinalPrice { get; set; }

    }
}
