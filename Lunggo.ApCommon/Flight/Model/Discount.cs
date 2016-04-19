namespace Lunggo.ApCommon.Flight.Model
{
    public class Discount
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string DisplayName { get; set; }
        public decimal Percentage { get; set; }
        public decimal Constant { get; set; }
        public decimal Nominal { get; set; }
    }
}