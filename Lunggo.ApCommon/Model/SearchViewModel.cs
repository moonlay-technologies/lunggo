using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Model
{
    public class SearchViewModel : SearchBase
    {
        public long? CountryCode { get; set; }
        public long? ProvinceCode { get; set; }
        public long? LargeCode { get; set; }
        public long? SmallCode { get; set; }
    }
}
