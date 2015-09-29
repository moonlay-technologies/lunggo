using System;
using System.ComponentModel.DataAnnotations;
using Lunggo.ApCommon.Payment.Constant;

namespace Lunggo.ApCommon.Payment.Model
{
    public class TransferConfirmationReport
    {
        [Required]
        public string RsvNo { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public DateTime PaymentTime { get; set; }
        [Required]
        public string RemitterName { get; set; }
        [Required]
        public string RemitterBank { get; set; }
        public string RemitterAccount { get; set; }
        [Required]
        public string BeneficiaryBank { get; set; }
        [Required]
        public string BeneficiaryAccount { get; set; }
        public string Message { get; set; }
        public string ReceiptUrl { get; set; }
        public TransferConfirmationReportStatus Status { get; set; }
    }
}