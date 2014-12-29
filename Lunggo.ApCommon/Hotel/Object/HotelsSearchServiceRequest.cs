using System;
using System.Collections.Generic;
using Lunggo.ApCommon.Hotel.Constant;
using Lunggo.ApCommon.Hotel.Model;

namespace Lunggo.ApCommon.Hotel.Object
{
    public class HotelsSearchServiceRequest : HotelSearchServiceRequestBase
    {
        public int LocationId { get; set; }
        public int StartIndex { get; set; }
        public int ResultCount { get; set; }
        public HotelsSearchFilter SearchFilter { get; set; }
        public HotelsSearchSortType SortBy { get; set; }
    }
}
