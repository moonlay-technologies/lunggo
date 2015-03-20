using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Hotel.Object
{
    public class HotelBookServiceRequest : HotelSearchServiceRequestBase
    {
        public String ClientIp { get; set; }
        public int HotelId { get; set; }
        public String PackageId { get; set; }
        public IEnumerable<String> RoomIdList { get; set; }
        public IEnumerable<String> RoomOccupantsName { get; set; } 
    }
}
