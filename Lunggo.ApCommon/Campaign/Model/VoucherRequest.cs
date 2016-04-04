using System;

namespace Lunggo.ApCommon.Campaign.Model
{
    public class VoucherRequest
    {
        public string VoucherCode { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
