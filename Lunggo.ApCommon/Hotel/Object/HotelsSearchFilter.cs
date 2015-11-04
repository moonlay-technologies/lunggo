using System.Collections.Generic;

namespace Lunggo.ApCommon.Hotel.Model
{
    public class HotelsSearchFilter
    {
        public IEnumerable<int> StarRatingsToDisplay { get; set; }
        public long MinPrice { get; set; }
        public long MaxPrice { get; set; }
    }
}
