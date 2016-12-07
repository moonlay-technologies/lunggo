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
        public List<Chain> chains { get; set; }
    }

    public class Chain
    {
        public string code { get; set; }
        public Description description { get; set; }
    }
}
