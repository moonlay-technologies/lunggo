using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Content.Model
{
    public class HotelRS
    {
        public List<string> providerDetails { get; set; }
        public List<HotelContent> hotels;
        public int from;
        public int to;
        public int total;
        //public Source source;
    }
}
