using System;
using System.Collections.Generic;
using Lunggo.ApCommon.Hotel.Model;

namespace Lunggo.ApCommon.Hotel.Object
{
    public class HotelsSearchServiceResponse
    {
        public IEnumerable<HotelDetail> HotelList { get; set; }
        public int TotalHotelCount { get; set; }
        public String SearchId { get; set; }
        public int TotalFilteredHotelCount { get; set; }
    }
}
