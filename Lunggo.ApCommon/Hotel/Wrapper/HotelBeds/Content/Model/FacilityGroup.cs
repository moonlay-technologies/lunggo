﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Content.Model
{
    public class FacilityGroupRS
    {
        public int total { get; set; }
        public List<FacilityGroupApi> facilityGroups { get; set; }
    }

    public class FacilityGroupApi
    {
        public int code { get; set; }
        public Description description { get; set; }
        public string DescriptionEng { get; set; }
        public string DescriptionInd { get; set; }
    }
}
