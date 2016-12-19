using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Content.Model
{
    public class CountryRS
    {
        public int total { get; set; }
        public List<CountryApi> countries { get; set; }
    }

    public class CountryApi
    {
        public string code { get; set; }
        public string isoCode { get; set; }
        public Description description { get; set; }
    }
}
