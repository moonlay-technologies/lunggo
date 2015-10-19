using Lunggo.ApCommon.Campaign.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Campaign.Model
{
    public class VoucherResponse
    {
        public decimal OriginalPrice { get; set; }
        public decimal DiscountedPrice { get; set; }
        public decimal TotalDiscount { get; set; }
        public VoucherValidationStatusType UpdateStatus { get; set; }
    }
}
