using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Campaign.Model
{
    public class CampaignVoucher
    {
        public String VoucherCode { get; set; }
        public long? CampaignId { get; set; }
        public int? RemainingCount { get; set; }
        public String CampaignName { get; set; }
        public String CampaignDescription { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public short? ValuePercentage { get; set; }
        public Decimal? ValueConstant { get; set; }
        public Decimal? MaxDiscountValue { get; set; }
        public Decimal? MinSpendValue { get; set; }
        public String CampaignTypeCd { get; set; }
        public bool? CampaignStatus { get; set; }
        public bool? IsSingleUsage { get; set; }
    }
}
