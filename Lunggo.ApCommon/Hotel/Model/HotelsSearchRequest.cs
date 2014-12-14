using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Hotel.Constant;

namespace Lunggo.ApCommon.Hotel.Model
{
    public class HotelsSearchRequest
    {
        public DateTime StayDate { get; set; }
        public int StayLength { get; set; }
        public String ResultCurrency { get; set; }
        public int LocationId { get; set; }
        public String Residency { get; set; }
        public IEnumerable<RoomOccupant> RoomOccupants { get; set; }
        public HotelSearchSortType SortBy { get; set; }

    }
}
