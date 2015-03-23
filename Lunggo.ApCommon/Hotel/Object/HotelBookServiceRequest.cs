using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Hotel.Model;

namespace Lunggo.ApCommon.Hotel.Object
{
    public class HotelBookServiceRequest : HotelSearchServiceRequestBase
    {
        public String ClientIp { get; set; }
        public int HotelId { get; set; }
        public String PackageId { get; set; }
        public String SessionId { get; set; }
        public String PackageDetail { get; set; }
        public IEnumerable<String> LeadRoomOccupantNames { get; set; } 
    }
}
