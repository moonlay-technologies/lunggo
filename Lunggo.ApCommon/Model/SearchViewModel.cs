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
