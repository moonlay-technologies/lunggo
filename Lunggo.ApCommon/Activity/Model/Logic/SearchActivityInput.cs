namespace Lunggo.ApCommon.Activity.Model.Logic
{
    public class SearchActivityInput
    {
        public ActivityFilter ActivityFilter { get; set; }
        public int Page { get; set; }
        public int PerPage { get; set; }
    }
}
