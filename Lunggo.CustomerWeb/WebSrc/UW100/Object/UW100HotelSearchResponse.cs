using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lunggo.ApCommon.Hotel.Constant;
using Lunggo.ApCommon.Hotel.Model;

namespace Lunggo.CustomerWeb.WebSrc.UW100.Object
{
    public class Uw100HotelSearchResponse
    {
        public String SearchId { get; set; }
        public String LocationName { get; set; }
        public String Country { get; set; }
        public String Province { get; set; }
        public String Area { get; set; }
        public int LocationId { get; set; }
        public DateTime StayDate { get; set; }
        public int StayLength { get; set; }
        public int StartIndex { get; set; }
        public int ResultCount { get; set; }
        public String Lang { get; set; }
        public HotelsSearchFilter SearchFilter { get; set; }
        public IEnumerable<RoomOccupant> RoomOccupants { get; set; }
        public HotelsSearchSortType SortBy { get; set; }
    }
}