using System;

namespace Lunggo.ApCommon.Payment.Model
{
    public class CampaignVoucher
    {
        public long CampaignId { get; set; }
        public int? RemainingCount { get; set; }
        public string CampaignName { get; set; }
        public string CampaignDescription { get; set; }
        public string DisplayName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal ValuePercentage { get; set; }
        public decimal ValueConstant { get; set; }
        public decimal MaxDiscountValue { get; set; }
        public decimal MinSpendValue { get; set; }
        public string ProductType { get; set; }
    }
}
