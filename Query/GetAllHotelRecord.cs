using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Query
{
    public class GetAllHotelRecord : QueryRecord
    {
        public String RsvNo { get; set; }
        public DateTime RsvTime  { get; set; }
        public String ContactName  { get; set; }
        public String RsvStatusCd  { get; set; }
        public String MemberCd  { get; set; }
        public String HotelNo  { get; set; }
        public DateTime CheckInDate  { get; set; }
        public DateTime CheckOutDate  { get; set; }

    }
}
