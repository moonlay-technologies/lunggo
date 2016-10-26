using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Hotel.Model
{
    public class HotelFilterDisplayInfo
    {
        public List<AccomodationFilter> AccomodationFilter { get; set; }
        public List<FacilitiesFilter> FacilityFilter { get; set; }
        public List<ZoneFilter> ZoneFilter { get; set; }
    }

    public class AccomodationFilter
    {
        public int Count { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }

    public class FacilitiesFilter
    {
        public int Count { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class ZoneFilter
    {
        public int Count { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
    }
}
