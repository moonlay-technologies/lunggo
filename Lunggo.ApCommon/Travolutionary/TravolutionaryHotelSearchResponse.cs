using System.Collections.Generic;

namespace Lunggo.ApCommon.Travolutionary
{
    public class TravolutionaryHotelSearchResponse : TravolutionaryResponseBase
    {
        public IEnumerable<int> HotelIdList { get; set; }
    }

}
