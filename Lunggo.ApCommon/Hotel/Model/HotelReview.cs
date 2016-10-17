using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Hotel.Model
{
    public class HotelReview
    {
        public decimal Rate { get; set; }
        public int ReviewCount { get; set; }
        public string Type { get; set; }
    }
}
