using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Actifity.Model;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Activity.Database.Query
{
    internal class ThingsToDoQuery : QueryBase<ThingsToDoQuery, ActivityForDisplayModel>
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
            clauseBuilder.Append("SELECT sub.* FROM (");
            clauseBuilder.Append("SELECT ActivityID, Name, ");
            clauseBuilder.Append("(SELECT MIN (Price) FROM ActivityDetails WHERE ActivityDetails.ActivityID ");
            clauseBuilder.Append("= ");
            clauseBuilder.Append("Activities.ActivityID) ");
            clauseBuilder.Append("AS Cheapest FROM Activities ");
            return clauseBuilder.ToString();
        }

        private static string CreateWhereClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append("WHERE Location like '%' + @city + '%'");
            clauseBuilder.Append(") AS Sub WHERE Cheapest != 0");
            return clauseBuilder.ToString();
        }
    }
}

