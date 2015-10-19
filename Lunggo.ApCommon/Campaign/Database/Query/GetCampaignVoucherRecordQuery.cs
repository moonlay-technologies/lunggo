using Lunggo.ApCommon.Campaign.Model;
using Lunggo.Framework.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Campaign.Database.Query
{
    public class GetCampaignVoucherRecordQuery : QueryBase<GetCampaignVoucherRecordQuery, CampaignVoucher>
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
            clauseBuilder.Append("SELECT [VoucherCode], [CampaignId], [RemainingCount], ");
            clauseBuilder.Append("[Name] AS CampaignName, [Description] AS CampaignDescription, ");
            clauseBuilder.Append("[StartDate], [EndDate], ");
            clauseBuilder.Append("[ValuePercentage], [ValueConstant], ");
            clauseBuilder.Append("[MaxDiscountValue], [MinSpendValue], ");
            clauseBuilder.Append("[CampaignTypeCd], [Status] AS CampaignStatus ");
            clauseBuilder.Append("FROM [Campaign] ");
            return clauseBuilder.ToString();
        }

        private static string CreateWhereClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append("WHERE VoucherCode = @VoucherCode");
            return clauseBuilder.ToString();
        }
    }
}
