namespace Lunggo.ApCommon.Payment.Model
{
    public class BinMethodDiscount
    {
        public decimal Amount { get; set; }
        public Currency Currency { get; set; }
        public string DisplayName { get; set; }
        public bool IsAvailable { get; set; }
        public bool ReplaceMargin { get; set; }
    }
}
