using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Content.Model
{
    public class AccomodationRS
    {
        public List<AccomodationApi> accomodations { get; set; }
        public int total { get; set; }
    }

    public class AccomodationApi
    {
        public string code { get; set; }
        public Description typeMultiDescription { get; set; }
        public string DescriptionEng { get; set; }
        public string DescriptionInd { get; set; }
        public string typeDescription { get; set; }
    }
}
