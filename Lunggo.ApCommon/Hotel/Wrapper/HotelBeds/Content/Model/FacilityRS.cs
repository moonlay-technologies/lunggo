using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Content.Model
{
    public class FacilityRS
    {
        public int total { get; set; }
        public List<FacilityApi> facilities { get; set; }
    }

    public class FacilityApi
    {
        public int code { get; set; }
        public int facilityGroupCode { get; set; }
        public int facilityTypologyCode { get; set; }
        public Description description { get; set; }
        public string DescriptionEng { get; set; }
        public string DescriptionInd { get; set; }
    }
}
