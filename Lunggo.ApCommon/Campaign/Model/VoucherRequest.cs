using System;

namespace Lunggo.ApCommon.Campaign.Model
{
    public class VoucherRequest
    {
        public String VoucherCode { get; set; }
        public String Email { get; set; }
        public decimal Price { get; set; }
    }
}
