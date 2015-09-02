namespace Lunggo.ApCommon.Flight.Model
{
    public class DiscountData
    {
        public string Code { get; set; }
        public long Id { get; set; }
        public decimal Coefficient { get; set; }
        public decimal Constant { get; set; }
        public decimal Nominal { get; set; }
    }
}