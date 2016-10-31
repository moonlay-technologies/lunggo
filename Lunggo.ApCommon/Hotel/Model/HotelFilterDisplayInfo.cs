using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsQuery.EquationParser.Implementation;

namespace Lunggo.ApCommon.Hotel.Model
{
    public class HotelFilterDisplayInfo
    {
        public List<AccomodationFilterInfo> AccomodationFilter { get; set; }
        public List<FacilitiesFilterInfo> FacilityFilter { get; set; }
        public List<ZoneFilterInfo> ZoneFilter { get; set; }
        public List<StarFilterInfo> StarFilter { get; set; }
        public List<AreaFilterInfo> AreaFilter { get; set; } 
    }

    public class AreaFilterInfo
    {
        public int Count { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }

    public class AccomodationFilterInfo
    {
        public int Count { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }

    public class FacilitiesFilterInfo
    {
        public int Count { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }

    public class ZoneFilterInfo
    {
        public int Count { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
    }

    public class StarFilterInfo
    {
        public int Count { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
    }
}
