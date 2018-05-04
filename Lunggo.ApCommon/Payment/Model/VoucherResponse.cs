using Lunggo.ApCommon.Payment.Constant;

namespace Lunggo.ApCommon.Payment.Model
{
    public class VoucherResponse
    {
        public decimal DiscountedPrice { get; set; }
        public decimal TotalDiscount { get; set; }
        public UsedDiscount Discount { get; set; }
    }
}
