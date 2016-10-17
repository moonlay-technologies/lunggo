using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Hotel.Model
{
    public class HotelSorting
    {
        public bool AscendingPrice { get; set; }
        public bool DescendingPrice { get; set; }
        public bool ByReview { get;set; }
        public bool ByPopularity { get; set; }
    }
}
