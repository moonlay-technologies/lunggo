using Lunggo.Framework.Database;
using System.Text;

namespace Lunggo.ApCommon.Campaign.Database.Query
{
    internal class UseBudgetQuery : ExecuteNonQueryBase<UseBudgetQuery>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append("UPDATE Campaign ");
            queryBuilder.Append("SET UsedBudget = UsedBudget + @Discount ");
            queryBuilder.Append("WHERE CampaignId = ");
            queryBuilder.Append("(SELECT CampaignId FROM CampaignVoucher WHERE VoucherCode = @VoucherCode)");
            return queryBuilder.ToString();
        }
    }
}
