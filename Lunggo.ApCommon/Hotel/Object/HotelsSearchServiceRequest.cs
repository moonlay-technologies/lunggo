using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Hotel.Constant;

namespace Lunggo.ApCommon.Hotel.Model
{
    public class HotelsSearchServiceRequest
    {
        public int LocationId { get; set; }
        public DateTime StayDate { get; set; }
        public int StayLength { get; set; }
        public String SearchId { get; set; }
        public int StartIndex { get; set; }
        public int ResultCount { get; set; }
        public String Lang { get; set; }
        public HotelsSearchFilter SearchFilter { get; set; }
        public IEnumerable<RoomOccupant> RoomOccupants { get; set; }
        public HotelsSearchSortType SortBy { get; set; }
    }
}
