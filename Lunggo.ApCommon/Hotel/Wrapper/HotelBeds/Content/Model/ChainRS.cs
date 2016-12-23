using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Content.Model
{
    public class ChainRS
    {
        public int total { get; set; }
        public List<ChainApi> chains { get; set; }
    }

    public class ChainApi
    {
        public string code { get; set; }
        public Description description { get; set; }
        public string DescriptionEng { get; set; }
        public string DescriptionInd { get; set; }
    }
}
