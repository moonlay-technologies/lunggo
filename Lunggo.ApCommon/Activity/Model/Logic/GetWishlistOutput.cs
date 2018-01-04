using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Activity.Model.Logic
{
    public class GetWishlistOutput
    {
        public List<SearchResult> ActivityList { get; set; }
        public int Page { get; set; }
        public int PerPage { get; set; }
    }
}
