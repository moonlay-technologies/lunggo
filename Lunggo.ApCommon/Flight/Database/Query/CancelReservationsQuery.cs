using System.Text;
using Lunggo.Framework.Database;

namespace Lunggo.ApCommon.Flight.Database.Query
{
    internal class CancelReservationsQuery : NoReturnQueryBase<CancelReservationsQuery>
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
            clauseBuilder.Append(@"SET PaymentStatusCd = 'CAN', ");
            clauseBuilder.Append(@"CancellationTime = GETUTCDATE(), ");
            clauseBuilder.Append(@"CancellationTypeCd = @CancellationType ");
            return clauseBuilder.ToString();
        }

        private string CreateWhereClause()
        {
            var clauseBuilder = new StringBuilder();
            clauseBuilder.Append(@"WHERE RsvNo = @RsvNo");
            return clauseBuilder.ToString();
        }
    }
}
