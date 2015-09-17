using System.Text;
using Lunggo.Framework.Database;
using Lunggo.Repository.TableRecord;

namespace Lunggo.ApCommon.Flight.Database.Query
{
    internal class GetSavedPassengersByContactEmailQuery : QueryBase<GetSavedPassengersByContactEmailQuery, FlightSavedPassengerTableRecord>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append(CreateSelectClause());
            queryBuilder.Append(CreateWhereClauser());
            return queryBuilder.ToString();
        }

        private static string CreateSelectClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append("SELECT * FROM FlightSavedPassenger ");
            return clauseBuilder.ToString();
        }

        private static string CreateWhereClauser()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append("WHERE ContactEmail = @ContactEmail");
            return clauseBuilder.ToString();
        }
    }
}
