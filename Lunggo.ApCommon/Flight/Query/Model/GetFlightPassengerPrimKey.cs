using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Flight.Query.Model
{
    internal class GetFlightPassengerPrimKey : QueryBase<GetFlightPassengerPrimKey, long>
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
            clauseBuilder.Append(@"SELECT PassengerId ");
            clauseBuilder.Append(@"FROM FlightPassenger ");
            return clauseBuilder.ToString();
        }

        private static string CreateWhereClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append(@"WHERE ");
            clauseBuilder.Append(@"FirstName = @FirstName AND ");
            clauseBuilder.Append(@"LastName = @LastName AND ");
            clauseBuilder.Append(@"BirthDate = @DateOfBirth AND ");
            clauseBuilder.Append(@"IdNumber = IdNumber");
            return clauseBuilder.ToString();
        }
    }
}
