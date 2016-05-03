using System;
using Lunggo.ApCommon.Campaign.Constant;
using Lunggo.ApCommon.Payment.Model;

namespace Lunggo.ApCommon.Campaign.Model
{
    public class VoucherResponse
    {
        public string VoucherCode { get; set; }
        public string Email { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal DiscountedPrice { get; set; }
        public decimal TotalDiscount { get; set; }
        public Discount Discount { get; set; }
        public VoucherStatus VoucherStatus { get; set; }
    }
}
