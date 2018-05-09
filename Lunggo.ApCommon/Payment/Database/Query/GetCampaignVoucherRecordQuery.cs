using System.Text;
using Lunggo.ApCommon.Payment.Model;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;

namespace Lunggo.ApCommon.Payment.Database.Query
{
    internal class GetCampaignVoucherRecordQuery : DbQueryBase<GetCampaignVoucherRecordQuery, CampaignVoucher, CampaignVoucherTableRecord, CampaignTableRecord>
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
            clauseBuilder.Append("SELECT A.*, B.* ");
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
