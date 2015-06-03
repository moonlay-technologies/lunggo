using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.ApCommon.Flight.Model;
using Lunggo.ApCommon.Flight.Query.Model;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;

namespace Lunggo.ApCommon.Flight.Query
{
    internal class GetFlightPassengerSummaryQuery : QueryBase<GetFlightPassengerSummaryQuery, FlightPassengerTableRecord>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append(CreateSelectClause());
            queryBuilder.Append(CreateWhereClause(condition));
            return queryBuilder.ToString();
        }

        private static string CreateSelectClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append(@"SELECT TitleCd, FirstName, LastName, PassengerTypeCd ");
            clauseBuilder.Append(@"FROM FlightPassenger ");
            return clauseBuilder.ToString();
        }

        private static string CreateWhereClause(dynamic condition)
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append(@"WHERE RsvNo = @RsvNo");
            return clauseBuilder.ToString();
        }
    }
}
