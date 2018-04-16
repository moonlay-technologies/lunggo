using System.Collections.Generic;

namespace Lunggo.ApCommon.Activity.Model.Logic
{
    public class GetListActivityOutput
    {
        public List<SearchResult> ActivityList { get; set; }
        public int Page { get; set; }
        public int PerPage { get; set; }
    }
}
