using System.Text;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Flight.Query
{
    internal class ExpireReservationsQuery : NoReturnQueryBase<ExpireReservationsQuery>
    {
        protected override string GetQuery(dynamic condition = null)
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append(CreateUpdateClause());
            queryBuilder.Append(CreateSetClause());
            queryBuilder.Append(CreateWhereClause());
            return queryBuilder.ToString();
        }

        private string CreateUpdateClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append(@"UPDATE FlightReservation ");
            return clauseBuilder.ToString();
        }

        private string CreateSetClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append(@"SET PaymentStatusCd = 'EXP' ");
            return clauseBuilder.ToString();
        }

        private string CreateWhereClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append(@"WHERE RsvTime < (DATEADD(MINUTE, @MinuteTimeout, GETUTCDATE()))");
            return clauseBuilder.ToString();
        }
    }
}
