using Lunggo.ApCommon.Activity.Constant;

namespace Lunggo.ApCommon.Activity.Model.Logic
{
    public class SearchActivityInput
    {
        public SearchActivityType SearchActivityType { get; set; }
        public string SearchId { get; set; }
        public ActivityFilter ActivityFilter { get; set; }
        //public string Name { get; set; }
        public int Page { get; set; }
        public int PerPage { get; set; }
    }
}
