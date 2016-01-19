namespace Lunggo.ApCommon.Flight.Model
{
    public class DiscountData
    {
        public string Code { get; set; }
        public long Id { get; set; }
        public string Name { get; set; }
        public decimal Percentage { get; set; }
        public decimal Constant { get; set; }
        public decimal Nominal { get; set; }
    }
}