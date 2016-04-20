using System;
using Lunggo.ApCommon.Payment.Constant;
using Lunggo.ApCommon.Flight.Model;

namespace Lunggo.ApCommon.Payment.Model
{
    public class PaymentInfo
    {
        public string Id { get; set; }
        public PaymentMedium Medium { get; set; }
        public PaymentMethod Method { get; set; }
        public PaymentStatus Status { get; set; }
        public DateTime? Time { get; set; }
        public DateTime? TimeLimit { get; set; }
        public Data Data { get; set; }
        public string TargetAccount { get; set; }
        public string Url { get; set; }
        public decimal FinalPrice { get; set; }
        public decimal PaidAmount { get; set; }
        public string Currency { get; set; }
        public string TransferToken { get; set; }
        public string DiscountCode { get; set; }
        public RefundInfo Refund { get; set; }
    }
}
