using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Content.Model
{
    public class FacilityGroupRS
    {
        public int total { get; set; }
        public List<FacilityGroup> facilityGroups { get; set; }
    }

    public class FacilityGroup
    {
        public string code { get; set; }
        public Description description { get; set; }
    }
}
