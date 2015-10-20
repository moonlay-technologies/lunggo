using Lunggo.ApCommon.Campaign.Model;
using Lunggo.Framework.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Campaign.Database.Query
{
    internal class GetCampaignVoucherRecordQuery : QueryBase<GetCampaignVoucherRecordQuery, CampaignVoucher>
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
            clauseBuilder.Append("B.[Name] AS CampaignName, B.[Description] AS CampaignDescription, ");
            clauseBuilder.Append("B.[StartDate], B.[EndDate], ");
            clauseBuilder.Append("B.[ValuePercentage], B.[ValueConstant], ");
            clauseBuilder.Append("B.[MaxDiscountValue], B.[MinSpendValue], ");
            clauseBuilder.Append("B.[CampaignTypeCd], B.[Status] AS CampaignStatus ");
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
