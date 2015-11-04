using System;

namespace Lunggo.ApCommon.Campaign.Model
{
    public class VoucherResponse
    {
        public String VoucherCode { get; set; }
        public String Email { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal DiscountedPrice { get; set; }
        public decimal TotalDiscount { get; set; }
        public String UpdateStatus { get; set; }
    }
}
