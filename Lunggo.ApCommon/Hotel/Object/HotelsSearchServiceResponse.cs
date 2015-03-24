using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
