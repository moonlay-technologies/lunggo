using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Hotel.Model;

namespace Lunggo.ApCommon.Travolutionary
{
    public class TravolutionaryHotelRoomSearchResponse : TravolutionaryResponseBase
    {
        public IEnumerable<RawRoomPackage> RoomPackages { get; set; }
    }
}
