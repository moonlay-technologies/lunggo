using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Hotel.Model
{
    public class HotelsSearchFilter
    {
        public IEnumerable<int> StarRatingsToDisplay { get; set; }
        public long MinPrice { get; set; }
        public long MaxPrice { get; set; }
    }
}
