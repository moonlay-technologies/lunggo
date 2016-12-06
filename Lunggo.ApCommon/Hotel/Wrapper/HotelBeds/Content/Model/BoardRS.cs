using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Hotel.Wrapper.HotelBeds.Content.Model
{
    public class BoardRS
    {
        public int total { get; set; }
        public List<Board> boards { get; set; } 
    }

    public class Board
    {
        public string code { get; set; }
        public string multiLingualCode { get; set; }
        public Description description { get; set; }
    }
}
