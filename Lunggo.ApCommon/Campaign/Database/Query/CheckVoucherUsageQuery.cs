using Lunggo.Framework.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lunggo.ApCommon.Campaign.Database.Query
{
    internal class CheckVoucherUsageQuery : QueryBase<CheckVoucherUsageQuery, int>
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
            clauseBuilder.Append("SELECT COUNT(RsvNo) ");
            clauseBuilder.Append("FROM [FlightReservation] ");
            return clauseBuilder.ToString();
        }

        private static string CreateWhereClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append("WHERE [VoucherCode] = @VoucherCode ");
            clauseBuilder.Append("AND [ContactEmail] = @Email");
            return clauseBuilder.ToString();
        }
    }
}
