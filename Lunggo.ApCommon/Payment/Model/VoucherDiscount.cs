using Lunggo.ApCommon.Payment.Constant;

namespace Lunggo.ApCommon.Payment.Model
{
    public class VoucherDiscount
    {
        public long CampaignId { get; set; }
        public string VoucherCode { get; set; }
        public decimal DiscountedPrice { get; set; }
        public decimal TotalDiscount { get; set; }
        public UsedDiscount Discount { get; set; }
    }
}
