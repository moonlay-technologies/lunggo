using System;
using Lunggo.ApCommon.Payment.Constant;

namespace Lunggo.ApCommon.Payment.Model
{
    public class TransferConfirmationReport
    {
        public long ReportId { get; set; }
        public string RsvNo { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentTime { get; set; }
        public string RemitterName { get; set; }
        public string RemitterBank { get; set; }
        public string RemitterAccount { get; set; }
        public string BeneficiaryBank { get; set; }
        public string BeneficiaryAccount { get; set; }
        public string Message { get; set; }
        public TransferConfirmationReportStatus Status { get; set; }
    }
}