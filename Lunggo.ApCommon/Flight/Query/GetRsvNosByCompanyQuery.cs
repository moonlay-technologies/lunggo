using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Flight.Query
{


    public class GetRsvNosByCompanyQuery : DbQueryBase<GetRsvNosByCompanyQuery, string>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append(CreateSelectClause());
            queryBuilder.Append(CreateWhereClause());
            queryBuilder.Append(CreateConditionClause(condition));
            return queryBuilder.ToString();
        }

        private static string CreateSelectClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append("SELECT r.RsvNo ");
            clauseBuilder.Append("FROM Reservation AS r ");
            clauseBuilder.Append("INNER JOIN [User] AS u ON r.UserId = u.Id  ");
            return clauseBuilder.ToString();
        }

        private static string CreateWhereClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append("WHERE u.CompanyId = @CompanyId AND r.RsvType = 'AGENT' ");
            return clauseBuilder.ToString();
        }

        private static string CreateConditionClause(dynamic condition)
        {
            var clauseBuilder = new StringBuilder();
                if (condition.BranchFilter != null)
                    clauseBuilder.Append(" AND u.Branch LIKE '%' + @BranchFilter + '%'");
                if (condition.DepartmentFilter != null)
                    clauseBuilder.Append(" AND u.Department LIKE '%' + @DepartmentFilter + '%'");
                if (condition.PositionFilter != null)
                    clauseBuilder.Append(" AND u.Position LIKE '%' + @PositionFilter + '%'");
            clauseBuilder.Append(" AND r.RsvTime >= @FromDate AND r.RsvTime <= @ToDate");
            clauseBuilder.Append(" ORDER BY r.RsvTime");
            return clauseBuilder.ToString();
        }
    }
}
