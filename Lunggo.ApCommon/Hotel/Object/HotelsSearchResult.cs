using System;
using System.Collections.Generic;

namespace Lunggo.ApCommon.Hotel.Object
{
    public class HotelsSearchResult
    {
        public IEnumerable<int> HotelIdList { get; set; }
        public String SearchId { get; set; }
    }
}
