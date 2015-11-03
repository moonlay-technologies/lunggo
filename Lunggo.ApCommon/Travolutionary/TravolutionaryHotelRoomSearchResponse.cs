using System.Collections.Generic;
using Lunggo.ApCommon.Hotel.Model;

namespace Lunggo.ApCommon.Travolutionary
{
    public class TravolutionaryHotelRoomSearchResponse : TravolutionaryResponseBase
    {
        public IEnumerable<RawRoomPackage> RoomPackages { get; set; }
    }
}
