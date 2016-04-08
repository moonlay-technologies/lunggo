using System;

namespace Lunggo.ApCommon.Campaign.Model
{
    public class ValidateVoucherRequest
    {
        public string VoucherCode { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
