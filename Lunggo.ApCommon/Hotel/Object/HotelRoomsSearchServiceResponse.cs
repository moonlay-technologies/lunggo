using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Hotel.Model;

namespace Lunggo.ApCommon.Hotel.Object
{
    public class HotelRoomsSearchServiceResponse
    {
        public IEnumerable<RoomPackage> RoomPackages { get; set; } 
    }
}
