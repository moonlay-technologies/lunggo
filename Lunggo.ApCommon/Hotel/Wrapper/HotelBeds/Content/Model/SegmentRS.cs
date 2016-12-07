using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Content.Model
{
    public class SegmentRS
    {
        public int total { get; set; }
        public List<Segment> segments { get; set; }
    }

    public class Segment
    {
        public int code { get; set; }
        public Description description { get; set; }
    }
}
