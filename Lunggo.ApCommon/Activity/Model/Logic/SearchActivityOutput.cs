using System.Collections.Generic;

namespace Lunggo.ApCommon.Activity.Model.Logic
{
    public class SearchActivityOutput
    {
        public string SearchId { get; set; }
        public List<SearchResult> ActivityList { get; set; }
        public int Page { get; set; }
        public int PerPage { get; set; }
    }
}
