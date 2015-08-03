using System;

namespace Lunggo.ApCommon.Payment.Model
{
    public class RefundInfo
    {
        public DateTime Time { get; set; }
        public decimal Amount { get; set; }
        public string TargetBank { get; set; }
        public string TargetAccount { get; set; }
    }
}