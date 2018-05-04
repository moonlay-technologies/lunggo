using System.Text;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Payment.Database.Query
{
    internal class GetCampaignVoucherRecordQuery : DbQueryBase<GetCampaignVoucherRecordQuery, CampaignVoucher>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append(CreateSelectClause());
            queryBuilder.Append(CreateWhereClause());
            return queryBuilder.ToString();
        }

        private static string CreateSelectClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append("SELECT A.[VoucherCode], A.[CampaignId], A.[RemainingCount], A.[IsSingleUsage], ");
            clauseBuilder.Append("B.[Name] AS CampaignName, B.[Description] AS CampaignDescription, B.DisplayName, ");
            clauseBuilder.Append("B.[StartDate], B.[EndDate], ");
            clauseBuilder.Append("B.[ValuePercentage], B.[ValueConstant], ");
            clauseBuilder.Append("B.[MaxDiscountValue], B.[MinSpendValue], ");
            clauseBuilder.Append("B.[CampaignTypeCd], B.[Status] AS CampaignStatus, ");
            clauseBuilder.Append("B.[ProductType], B.[MaxBudget], B.[UsedBudget] ");
            clauseBuilder.Append("FROM [CampaignVoucher] A ");
            clauseBuilder.Append("LEFT JOIN [Campaign] B ");
            clauseBuilder.Append("ON A.CampaignId = B.CampaignId ");
            return clauseBuilder.ToString();
        }

        private static string CreateWhereClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append("WHERE A.VoucherCode = @VoucherCode");
            return clauseBuilder.ToString();
        }
    }
}
