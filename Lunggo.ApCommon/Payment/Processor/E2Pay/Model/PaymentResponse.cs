using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Payment.Processor
{
    internal partial class PaymentProcessorService
    {
        private class PaymentResponse
        {
            private string MerchantCode { get; set; }
            private int PaymentId { get; set; }
            private string RefNo { get; set; }
            private long Amount { get; set; }
            private string Currency { get; set; }
            private string Remark { get; set; }
            private string TransId { get; set; }
            private string AuthCode { get; set; }
            private string Status { get; set; }
            private string ErrDesc { get; set; }
            private string Signature { get; set; }
        }
    }
}
