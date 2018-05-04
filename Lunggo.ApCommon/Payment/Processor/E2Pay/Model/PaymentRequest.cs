using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Payment.Processor
{
    internal partial class PaymentProcessorService
    {
        private class PaymentRequest
        {
            private string MerchantCode { get; set; }
            private int PaymentId { get; set; }
            private string RefNo { get; set; }
            private long Amount { get; set; }
            private string Currency { get; set; }
            private string ProdDesc { get; set; }
            private string UserName { get; set; }
            private string UserEmail { get; set; }
            private string UserContact { get; set; }
            private string Remark { get; set; }
            private string Lang { get; set; }
            private string Signature { get; set; }
            private string ResponseURL { get; set; }
            private string BackendURL { get; set; }
        }
    }
}
