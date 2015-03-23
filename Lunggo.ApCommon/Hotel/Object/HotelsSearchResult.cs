using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Hotel.Object
{
    public class HotelsSearchResult
    {
        public IEnumerable<int> HotelIdList { get; set; }
        public String SearchId { get; set; }
    }
}
