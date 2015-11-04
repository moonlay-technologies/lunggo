namespace Lunggo.ApCommon.Model
{
    public abstract class SearchBase
    {
        public int? ChildCount { get; set; }
        public int? AdultCount { get; set; }
        public int? ChkInDay { get; set; }
        public int? ChkInMonth { get; set; }
        public int? ChkInYear { get; set; }
        public int? StayCount { get; set; }
    }
}
