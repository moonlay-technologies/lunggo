using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Travolutionary
{
    public class TravolutionaryHotelSearchResponse : TravolutionaryResponseBase
    {
        public IEnumerable<int> HotelIdList { get; set; }
    }

}
