using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.Framework.Database;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.Repository.TableRecord;

namespace Lunggo.ApCommon.Flight.Query
{
    internal class GetFlightActivePriceMarginRuleQuery : QueryBase<GetFlightActivePriceMarginRuleQuery, FlightPriceMarginRuleTableRecord>
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
            clauseBuilder.Append("SELECT * ");
            clauseBuilder.Append("FROM FlightPriceMarginRule ");
            return clauseBuilder.ToString();
        }

        private static string CreateWhereClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append("WHERE IsActive = TRUE");
            return clauseBuilder.ToString();
        }
    }
}
