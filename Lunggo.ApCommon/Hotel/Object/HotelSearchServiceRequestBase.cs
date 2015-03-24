using System;
using System.Collections.Generic;
using Lunggo.ApCommon.Hotel.Model;

namespace Lunggo.ApCommon.Hotel.Object
{
    public abstract class HotelSearchServiceRequestBase
    {
        public DateTime StayDate { get; set; }
        public int StayLength { get; set; }
        public String SearchId { get; set; }
        public String Lang { get; set; }
        public IEnumerable<RoomOccupant> RoomOccupants { get; set; }
    }
}
