using Lunggo.ApCommon.Payment.Constant;

namespace Lunggo.ApCommon.Payment.Model
{
    public class VoucherResponse
    {
        public string VoucherCode { get; set; }
        public string Email { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal DiscountedPrice { get; set; }
        public decimal TotalDiscount { get; set; }
        public UsedDiscount Discount { get; set; }
        public VoucherStatus VoucherStatus { get; set; }
    }
}
