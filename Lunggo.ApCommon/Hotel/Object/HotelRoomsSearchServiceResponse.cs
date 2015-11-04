using System.Collections.Generic;
using Lunggo.ApCommon.Hotel.Model;

namespace Lunggo.ApCommon.Hotel.Object
{
    public class HotelRoomsSearchServiceResponse
    {
        public IEnumerable<RoomPackage> RoomPackages { get; set; } 
    }
}
