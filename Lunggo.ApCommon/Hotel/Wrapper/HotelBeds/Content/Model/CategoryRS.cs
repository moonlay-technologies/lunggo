using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Content.Model
{
    public class CategoryRS
    {
        public int total { get; set; }
        public List<CategoryApi> categories { get; set; }
    }

    public class CategoryApi
    {
        public string code { get; set; }
        public int simpleCode { get; set; }
        public string group { get; set; }
        public string accomodationType { get; set; }
        public Description description { get; set; }
        public string DescriptionEng { get; set; }
        public string DescriptionInd { get; set; }
    }
}
